using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Net;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.LocationParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IDbService dbService;

        public LocationController(ILocationService locationService, IDbService dbService)
        {
            this.locationService = locationService;
            this.dbService = dbService;

            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit; //runs on exit   
        }

        [HttpGet("GetUserLocationList")]
        public async Task<JsonResult> GetUserLocationList()
        {
            return await locationService.GetUserLocationList();
        }

        [HttpPost("AddLocation")]
        public async Task<bool> AddLocation(AddLocationRequest request)
        {
            return await locationService.AddLocation(request);
        }

        [HttpDelete("DeleteLocationByLocationID")]
        public async Task<bool> DeleteLocationByLocationID([FromQuery] int locationID)
        {
            return await locationService.DeleteLocationByLocationID(locationID);
        }

        //Support
        [NonAction]
        private void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }

    }
}
