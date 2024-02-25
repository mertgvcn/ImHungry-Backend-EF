using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.UserAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //general user info
        [HttpGet("GetUserInfo")]
        public async Task<JsonResult> GetUserInfo()
        {
            return await userService.GetUserInfo();
        }

        //account operations
        [HttpGet("GetAccountInfo")]
        public async Task<JsonResult> GetAccountInfo()
        {
            return await userService.GetAccountInfo();
        }

        [HttpPut("SetAccountInfo")]
        public async Task SetAccountInfo([FromBody] UserAccountViewModel request)
        {
            await userService.SetAccountInfo(request);
        }

        //location operations
        [HttpGet("GetCurrentLocation")]
        public async Task<JsonResult> GetCurrentLocation()
        {
            return await userService.GetCurrentLocation();
        }

        [HttpPut("SetCurrentLocation")]
        public async Task SetCurrentLocation([FromBody] SetCurrentLocationRequest request)
        {
            await userService.SetCurrentLocation(request);
        }

        //Verify given user property
        [HttpPost("VerifyUsername")]
        public async Task<bool> VerifyUsername([FromBody] VerifyUsernameRequest request)
        {
            return await userService.VerifyUsername(request);
        }

        [HttpPost("VerifyEmail")]
        public async Task<bool> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            return await userService.VerifyEmail(request);
        }

        [HttpPost("VerifyPassword")]
        public async Task<bool> VerifyPassword([FromBody] VerifyPasswordRequest request)
        {
            return await userService.VerifyPassword(request);
        }

        //Password operations
        [HttpPut("iForgotMyPassword")]
        public async Task<bool> iForgotMyPassword([FromBody] iForgotMyPasswordRequest request)
        {
            return await userService.iForgotMyPassword(request);
        }

        [HttpPut("ChangePassword")]
        public async Task ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await userService.ChangePassword(request);
        }
    }
}
