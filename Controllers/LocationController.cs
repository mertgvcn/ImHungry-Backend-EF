using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;  
        }

        [HttpGet("GetUserLocationList")]
        public async Task<JsonResult> GetUserLocationList()
        {
            return await locationService.GetUserLocationList();
        }

        [HttpPost("AddLocation")]
        public async Task AddLocation([FromBody] AddLocationRequest request)
        {
            await locationService.AddLocation(request);
        }

        [HttpDelete("DeleteLocationByLocationID")]
        public async Task DeleteLocationByLocationID([FromQuery] long locationID)
        {
            await locationService.DeleteLocationByLocationID(locationID);
        }

    }
}
