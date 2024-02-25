using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.UserAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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
