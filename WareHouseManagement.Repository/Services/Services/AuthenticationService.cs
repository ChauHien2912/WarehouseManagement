using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Authen;
using WareHouseManagement.Repository.Dtos.Response.Authen;
using WareHouseManagement.Repository.Entities;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.Repository.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _uof;

        public AuthenticationService(IMapper mapper, IConfiguration config, IUnitOfWork uof)
        {
            _mapper = mapper;
            _config = config;
            _uof = uof;
        }

        public async Task<FetchUserResponse> FetchUser(string accessToken)
        {
            var refreshTokenEntity = await _uof.GetRepository<RefreshTokenAccount>()
                 .SingleOrDefaultAsync(
                                         predicate: x => x.AccessToken.Equals(accessToken) && x.Expires >= DateTime.Now,
                                         include: x => x.Include(y => y.Account.Role));
            if (refreshTokenEntity == null)
            {
                throw new Exception("Không tìm thấy access token!");
            }
            var account = refreshTokenEntity.Account;
            Warehouse warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
            Shipper shipper = await _uof.GetRepository<Shipper>().SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
            if (warehouse == null && shipper == null)
            {
                throw new Exception("Không tìm thấy tài khoản");
            }
            return new FetchUserResponse()
            {
                //AccountId = account.I,
                RoleName = account.Role?.RoleName,
                ShipperResponse = shipper != null ? _mapper.Map<ShipperLoginResponse>(shipper) : null,
                WarehouseResponse = warehouse != null ? _mapper.Map<WarehouseLoginResponse>(warehouse) : null
            };
        }

        public async Task<LoginResponse> Login(string email, string password)
        {

            var account = await _uof.GetRepository<Account>().SingleOrDefaultAsync(
        predicate: u => u.Email == email,
        include: x => x.Include(a => a.Role));

            // Return null if user not found
            if (account == null)
            {
                return null;
            }

            // Kiểm tra mật khẩu băm
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.Password);
            if (!isPasswordValid)
            {
                return null;
            }

            Warehouse warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
            Shipper shipper = await _uof.GetRepository<Shipper>().SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
            Admin admin = await _uof.GetRepository<Admin>().SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
            if (warehouse == null && shipper == null && admin == null)
            {
                throw new Exception("Không tìm thấy tài khoản");
            }

            // Authentication successful, so generate JWT token and refresh token
            var accessToken = GenerateToken(account.Email, account.Role.RoleName, account.Id);
            var refreshToken = GenerateRefreshToken();
            var newRefreshToken = new RefreshTokenAccount()
            {
                Id = Guid.NewGuid(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccountId = account.Id,
                Expires = DateTime.UtcNow.AddDays(30),
            };
            await _uof.GetRepository<RefreshTokenAccount>().InsertAsync(newRefreshToken);
            bool isInsert = await _uof.CommitAsync() > 0;
            if (!isInsert)
            {
                throw new Exception("Cannot insert token to DB");
            }

            return new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccountId = account.Id,
                RoleName = account.Role?.RoleName,
                ShipperResponse = shipper != null ? _mapper.Map<ShipperLoginResponse>(shipper) : null,
                WarehouseResponse = warehouse != null ? _mapper.Map<WarehouseLoginResponse>(warehouse) : null,
                AdminResponse = admin != null ? _mapper.Map<AdminLoginResponse>(admin) : null,
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GenerateToken(string username, string roleName, Guid accountid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JWT:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim("AccountId", accountid.ToString())
                }),

                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> Logout(LogoutRequest logoutRequest)
        {
            var refreshTokens = await _uof.GetRepository<RefreshTokenAccount>().GetListAsync(predicate: x => x.RefreshToken.Equals(logoutRequest.RefreshToken));
            if (refreshTokens == null)
            {
                throw new Exception("Refresh token not found!");
            }
            _uof.GetRepository<RefreshTokenAccount>().DeleteRangeAsync(refreshTokens);
            bool isDelete = await _uof.CommitAsync() > 0;
            return isDelete;
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshTokenEntity = await _uof.GetRepository<RefreshTokenAccount>().SingleOrDefaultAsync(
                                                predicate: x => x.RefreshToken == refreshTokenRequest.RefreshToken
                                                && x.Expires >= DateTime.Now);
            if (refreshTokenEntity == null)
            {
                throw new Exception("RefreshToken not found or expired");
            }

            var account = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Id == refreshTokenEntity.AccountId);
            if (account == null)
            {
                throw new Exception("Account not found or expired");
            }

            var accessToken = GenerateToken(account.Email, account.RoleId.ToString(), account.Id);
            refreshTokenEntity.AccessToken = accessToken;
            _uof.GetRepository<RefreshTokenAccount>().UpdateAsync(refreshTokenEntity);
            bool isUpdate = await _uof.CommitAsync() > 0;
            if (!isUpdate)
            {
                throw new Exception("Cannot insert new access token to DB");
            }
            return new RefreshTokenResponse() { AccessToken = refreshTokenEntity.AccessToken, RefreshToken = refreshTokenEntity.RefreshToken };
        }
    }
}
