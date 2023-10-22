using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Models.Parameters.UserParams;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IUserService
    {
        public Task<JsonResult> GetUserInfo();
        public int GetCurrentUserID();

        public Task<JsonResult> GetAccountInfo();
        public Task<bool> SetAccountInfo(SetAccountInfoRequest request);

        public Task<JsonResult> GetCurrentLocation();
        public Task<bool> SetCurrentLocation(SetCurrentLocationRequest request);

        public Task<bool> VerifyUsername(VerifyUsernameRequest request);
        public Task<bool> VerifyEmail(VerifyEmailRequest request);
        public Task<bool> VerifyPassword(VerifyPasswordRequest request);

        public Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request);
        public Task<bool> ChangePassword(ChangePasswordRequest request);
    }
}
