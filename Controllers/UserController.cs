using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task SetCurrentLocation([FromBody] long locationID)
        {
            await userService.SetCurrentLocation(locationID);
        }

        //Verify given user property
        [HttpPost("VerifyUsername")]
        public async Task<bool> VerifyUsername([FromBody] string username)
        {
            return await userService.VerifyUsername(username);
        }

        [HttpPost("VerifyEmail")]
        public async Task<bool> VerifyEmail([FromBody] string email)
        {
            return await userService.VerifyEmail(email);
        }

        [HttpPost("VerifyPassword")]
        public async Task<bool> VerifyPassword([FromBody] string password)
        {
            return await userService.VerifyPassword(password);
        }

        //Password operations
        [HttpPut("iForgotMyPassword")]
        public async Task<bool> iForgotMyPassword([FromBody] iForgotMyPasswordRequest request)
        {
            return await userService.iForgotMyPassword(request);
        }

        [HttpPut("ChangePassword")]
        public async Task ChangePassword([FromBody] string encryptedPassword)
        {
            await userService.ChangePassword(encryptedPassword);
        }
    }
}
