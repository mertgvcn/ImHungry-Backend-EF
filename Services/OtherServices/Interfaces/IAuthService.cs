using ImHungryBackendER.Models.ParameterModels;

namespace WebAPI_Giris.Services.OtherServices.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest user);
        Task<long> GetIdByUsername(string username);
    }
}
