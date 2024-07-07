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
        Task<bool> DeleteAccountById(Guid id);
        Task<bool> ChangePassword(Guid id, ChangedPasswordRequest changePasswordRequest);
        Task<GetAccountResponse> GetAccountById(Guid id);

        Task<bool> UpdateAccount(Guid id, UpdateAccountRequest updateAccountRequest);

        
    }
}
