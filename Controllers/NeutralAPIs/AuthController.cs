using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.NeutralAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginUserAsync(request);

            return result;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserRegisterResponse>> Register([FromBody] UserRegisterRequest user)
        {
            var result = await authService.RegisterUserAsync(user);

            return result;
        }
    }
}
