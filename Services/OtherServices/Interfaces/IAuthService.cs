using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.AuthParams;

namespace WebAPI_Giris.Services.OtherServices.Interfaces
{
    public interface IAuthService
    {
        public Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        public Task<UserRegisterResponse> RegisterUserAsync(Users user);
        public Task<int> GetIdByUsername(string username);
    }
}
