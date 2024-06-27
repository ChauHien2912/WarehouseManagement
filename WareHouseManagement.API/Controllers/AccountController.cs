using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Account;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Account.AccountEndPoint + "/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        public AccountController(IMapper mapper, IAccountService accountService) : base(mapper)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount([FromBody] CreateAccountRequest createAccountRequest)
        {
            try
            {
                var accoutResponse = await _accountService.RegisterAccount(createAccountRequest);
                if (accoutResponse == null)
                {
                    return BadRequest("Cannot register account!");
                }
                return Ok(accoutResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangedPasswordRequest changePasswordRequest)
        {
            try
            {
                var isChangePassword = await _accountService.ChangePassword(id, changePasswordRequest);
                if (!isChangePassword)
                {
                    return BadRequest("Cannot change password!");
                }
                return Ok("Change password successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            try
            {
                var isDelete = await _accountService.DeleteAccountById(id);
                if (!isDelete)
                {
                    return BadRequest("Cannot delete this account!");
                }
                return Ok("Delete account successfully!");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAccountById([FromRoute]Guid id)
        {
            try
            {
                var accooutReponse = await _accountService.GetAccountById(id);
                if (accooutReponse == null)
                {
                    return BadRequest("Cannot find this account!");
                }
                return Ok(accooutReponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
