using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IUserService
    {
        public Task<JsonResult> GetUserInfo();
        public long GetCurrentUserID();

        public Task<JsonResult> GetAccountInfo();
        public Task SetAccountInfo(UserAccountViewModel request);

        public Task<JsonResult> GetCurrentLocation();
        public Task SetCurrentLocation(SetCurrentLocationRequest request);

        public Task<bool> VerifyUsername(VerifyUsernameRequest request);
        public Task<bool> VerifyEmail(VerifyEmailRequest request);
        public Task<bool> VerifyPassword(VerifyPasswordRequest request);

        public Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request);
        public Task ChangePassword(ChangePasswordRequest request);
    }
}
