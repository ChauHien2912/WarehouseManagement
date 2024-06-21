using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.User;
using WareHouseManagement.Repository.Dtos.Response.User;
using WareHouseManagement.Repository.Models;

//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IUserService
    {
        public Account Login(string email, string password);

        public string GenerateJWTtoken(Account account);

        public bool IsUser(string email, string password);

        public Task<bool> RegisterAccount(CreateUserRequest request);


        public Task<bool> DeleteUser(int id);

        public Task<bool> UpdateUser(int id, UpdateUserRequest request);

        public Task<bool> ChangedPass(int id, string oldPass, string newPass);


        Task<IPaginate<GetAccountResponse>> GetAccountAsync(int page, int size);
    }
}
