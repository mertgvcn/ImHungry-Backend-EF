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
        public Task SetCurrentLocation(long locationID);

        public Task<bool> VerifyUsername(string username);
        public Task<bool> VerifyEmail(string email);
        public Task<bool> VerifyPassword(string password);

        public Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request);
        public Task ChangePassword(string encryptedPassword);
    }
}
