using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.UserParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IDbService dbService;

        public UserController(IUserService userService, IDbService dbService)
        {
            this.userService = userService;
            this.dbService = dbService;
            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit; //runs on exit       
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
        public async Task<bool> SetAccountInfo([FromBody] SetAccountInfoRequest request)
        {
            return await userService.SetAccountInfo(request);
        }

        //location operations
        [HttpGet("GetCurrentLocation")]
        public async Task<JsonResult> GetCurrentLocation()
        {
            return await userService.GetCurrentLocation();
        }

        [HttpPut("SetCurrentLocation")]
        public async Task<bool> SetCurrentLocation([FromBody] SetCurrentLocationRequest request)
        {
            return await userService.SetCurrentLocation(request);
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
        public async Task<bool> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await userService.ChangePassword(request);
        }

        //Helpers
        [NonAction]
        public void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }
    }
}
