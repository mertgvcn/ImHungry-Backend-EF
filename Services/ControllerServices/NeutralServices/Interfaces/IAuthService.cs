using ImHungryBackendER.Models.ParameterModels;

namespace ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest user);
        Task<long> GetIdByUsername(string username);
    }
}
