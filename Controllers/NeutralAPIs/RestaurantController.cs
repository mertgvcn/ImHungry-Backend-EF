using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.NeutralAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [HttpPost("GetRestaurantInfoByID")]
        public async Task<JsonResult> GetRestaurantInformationByID([FromBody] int restaurantID)
        {
            return await restaurantService.GetRestaurantInformationByID(restaurantID);
        }

        [HttpPost("GetRestaurantListByLocation")]
        public async Task<JsonResult> GetRestaurantListByLocation([FromBody] GetRestaurantListByLocationRequest request)
        {
            return await restaurantService.GetRestaurantListByLocation(request);
        }

    }
}
