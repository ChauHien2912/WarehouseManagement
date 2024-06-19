using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.User;
using WareHouseManagement.Repository.Dtos.Response.User;
using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IConfiguration config, IUserRepository userRepository, IMapper mapper)
        {
            _uow = uow;
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> ChangedPass(int id, string oldPass, string newPass)
        {
            bool isUpdate = false;
            var existing = await _uow.GetRepository<Account>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existing == null)
            {
                throw new Exception("Cannot Find User");
            }
            if (existing.Password == oldPass)
            {
                existing.Password = newPass;
                 _uow.GetRepository<Account>().UpdateAsync(existing);
                bool isUpdated = await _uow.CommitAsync() > 0;
                isUpdated = true;
            }
            return isUpdate;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var existing = await _uow.GetRepository<Account>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existing == null)
            {
                throw new Exception("Cannot Find User");
            }
            _uow.GetRepository<Account>().DeleteAsync(existing);
            bool isDeleted = await _uow.CommitAsync() > 0;
            return isDeleted;
        }

        public string GenerateJWTtoken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                            _config["Jwt:Issuer"],
                            claims,
                            expires: DateTime.Now.AddMinutes(1),
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public  async Task<IPaginate<GetAccountResponse>> GetAccountAsync(int page, int size)
        {
            var account = await _uow.GetRepository<Account>()
           .GetPagingListAsync(
               include: query => query.Include(s => s.Role),
               page: page,
               size: size
           );

            var accountResponses = new Paginate<GetAccountResponse>()
            {
                Page = account.Page,
                Size = account.Size,
                Total = account.Total,
                TotalPages = account.TotalPages,
                Items = _mapper.Map<IList<GetAccountResponse>>(account.Items),
            };
            return accountResponses;
        }

        public bool IsUser(string username, string password)
        {
            return _userRepository.GetAccount(username, password) != null;
        }

        public Account Login(string username, string password)
        {
            var user = _userRepository.GetAccount(username, password);
            return user;
        }

        public async Task<bool> RegisterAccount(CreateUserRequest request)
        {
            try
            {
                var user = _mapper.Map<Account>(request);
                await _uow.GetRepository<Account>().InsertAsync(user);
                bool isCreated = await _uow.CommitAsync() > 0;
                return isCreated;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            } 
        }

        public async Task<bool> UpdateUser(int id, UpdateUserRequest request)
        {
            var existing = await _uow.GetRepository<Account>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existing == null)
            {
                throw new Exception("Cannot Find User");
            }
            existing = _mapper.Map<Account>(request);
            _uow.GetRepository<Account>().UpdateAsync(existing);
            bool isUpdated = await _uow.CommitAsync() > 0;
            return isUpdated;
        }        
    }
}
