using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Account;
using WareHouseManagement.Repository.Dtos.Response.Account;
using WareHouseManagement.Repository.Dtos.Response.User;
using WareHouseManagement.Repository.Enum;
using WareHouseManagement.Repository.Models;

//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.Repository.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork uof, IConfiguration config, IMapper mapper)
        {
            _uof = uof;
            _config = config;
            _mapper = mapper;
        }

        public async Task<bool> ChangePassword(int id, ChangedPasswordRequest changePasswordRequest)
        {
            var account = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Id == id);
            if (!changePasswordRequest.CurrentPassword.Equals(account.Password))
            {
                throw new Exception("Current password is not correct!");
            }
            account.Password = changePasswordRequest.NewPassword;
            _uof.GetRepository<Account>().UpdateAsync(account);
            return await _uof.CommitAsync() > 0;
        }

        public async Task<bool> DeleteAccountById(int id)
        {
            var account = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Id == id);
            account.IsActive = false;
            _uof.GetRepository<Account>().UpdateAsync(account);
            return await _uof.CommitAsync() > 0;
        }

        public async Task<GetAccountResponse> GetAccountById(int id)
        {
            GetAccountResponse response = new GetAccountResponse();
            Account account = await _uof
                .GetRepository<Account>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: source => source.Include(a => a.Role)
                 );
            if (account == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }
            if (account.Role.RoleName.Equals(RoleEnum.Shipper.ToString()))
            {
                Shipper shipper = await _uof.GetRepository<Shipper>()
                                                    .SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
                if (shipper == null)
                {
                    throw new Exception("Tài khoản không tồn tại");
                }
                response = _mapper.Map<GetAccountResponse>(shipper);
            }
            else if (account.Role.RoleName.Equals(RoleEnum.Warehouse.ToString()))
            {
                Warehouse warehouse = await _uof.GetRepository<Warehouse>()
                                    .SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id);
                if (warehouse == null)
                {
                    throw new Exception("Tài khoản không tồn tại");
                }
                response = _mapper.Map<GetAccountResponse>(warehouse);
            }
            else
            {
                throw new Exception("Role không tồn tại");
            }
            response = _mapper.Map(account, response);
            return response;
        }

        public async Task<CreateAccountResponse> RegisterAccount(CreateAccountRequest createAccountRequest)
        {
            CreateAccountResponse createAccountResponse = new CreateAccountResponse();
            var role = await _uof.GetRepository<Role>().SingleOrDefaultAsync(predicate: x => x.RoleName.Equals(createAccountRequest.RoleName));
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            var userName = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Email.Equals(createAccountRequest.Email));
            if (userName != null)
            {
                throw new Exception("Username đã tồn tại!");
            }
            var account = _mapper.Map<Account>(createAccountRequest);
            if (RoleEnum.Shipper.ToString().Equals(createAccountRequest.RoleName))
            {
                var userInfor = _mapper.Map<Shipper>(createAccountRequest);
                account.RoleId = role.Id;
                account.IsActive = true;
                userInfor.AccountId = account.Id;
                await _uof.GetRepository<Shipper>().InsertAsync(userInfor);
            }
            else if (RoleEnum.Warehouse.ToString().Equals(createAccountRequest.RoleName))
            {
                var warehouse = _mapper.Map<Warehouse>(createAccountRequest);
                account.RoleId = role.Id;
                account.IsActive = true;
                warehouse.AccountId = account.Id;
                await _uof.GetRepository<Warehouse>().InsertAsync(warehouse);
            }
            await _uof.GetRepository<Account>().InsertAsync(account);
            await _uof.CommitAsync();

            return _mapper.Map(createAccountRequest, createAccountResponse);
        }

        
    }
}
