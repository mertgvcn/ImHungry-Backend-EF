using ImHungryBackendER.Models.ParameterModels;

namespace WebAPI_Giris.Services.OtherServices.Interfaces
{
    public interface IAuthService
    {
        public Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        public Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest user);
        public Task<long> GetIdByUsername(string username);
    }
}
