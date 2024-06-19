using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Account;
using WareHouseManagement.Repository.Dtos.Response.Account;
using WareHouseManagement.Repository.Dtos.Response.User;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IAccountService
    {
        Task<CreateAccountResponse> RegisterAccount(CreateAccountRequest createAccountRequest);
        Task<bool> DeleteAccountById(int id);
        Task<bool> ChangePassword(int id, ChangedPasswordRequest changePasswordRequest);
        Task<GetAccountResponse> GetAccountById(int id);
    }
}
