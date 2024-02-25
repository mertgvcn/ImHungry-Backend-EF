using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserInfo();
        long GetCurrentUserID();
        List<string> GetCurrentUserRoles();

        Task<JsonResult> GetAccountInfo();
        Task SetAccountInfo(UserAccountViewModel request);

        Task<JsonResult> GetCurrentLocation();
        Task SetCurrentLocation(SetCurrentLocationRequest request);

        Task<bool> VerifyUsername(VerifyUsernameRequest request);
        Task<bool> VerifyEmail(VerifyEmailRequest request);
        Task<bool> VerifyPassword(VerifyPasswordRequest request);

        Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request);
        Task ChangePassword(ChangePasswordRequest request);
    }
}
