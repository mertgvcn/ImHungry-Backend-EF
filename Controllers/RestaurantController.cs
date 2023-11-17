using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
