using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.AuthParams;
using WebAPI_Giris.Services;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginUserAsync(request);

            return result;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserRegisterResponse>> Register(Users user)
        {
            var result = await authService.RegisterUserAsync(user);

            return result;
        }
    }
}
