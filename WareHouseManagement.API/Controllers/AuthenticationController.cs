using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Authen;
using WareHouseManagement.Repository.Dtos.Request.User;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Authentication.AuthenEndPoint + "/[action]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IMapper _mapper, IAuthenticationService authenticationService) : base(_mapper)
        {
            _authenticationService = authenticationService;
        }

        // POST api/user/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginAccountRepuest loginRequest)
        {
            try
            {
                var loginResponse = await _authenticationService.Login(loginRequest.Email, loginRequest.Password);

                if (loginResponse == null || String.IsNullOrWhiteSpace(loginResponse.ToString()))
                    return Unauthorized(new { message = "User name or password is incorrect" });
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var refreshTokenResponse = await _authenticationService.RefreshToken(request);
                return Ok(refreshTokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> LogOut(LogoutRequest logoutRequest)
        {
            try
            {
                bool isLogout = await _authenticationService.Logout(logoutRequest);
                if (!isLogout)
                {
                    return BadRequest("Cannot logout account!");
                }
                return Ok("Logout successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{accessToken}")]
        public async Task<IActionResult> FetchUser(string accessToken)
        {
            try
            {
                var response = await _authenticationService.FetchUser(accessToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
