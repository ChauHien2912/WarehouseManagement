using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Authen;
using WareHouseManagement.Repository.Dtos.Response.Authen;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> Login(string email, string password);
        public Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        public Task<bool> Logout(LogoutRequest logoutRequest);
        public Task<FetchUserResponse> FetchUser(string accessToken);
    }
}
