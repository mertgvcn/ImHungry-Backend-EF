using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.RestaurantManagementAPIs
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantManagerController : Controller
    {
        private readonly IRestaurantManagerService _restaurantManagerService;

        public RestaurantManagerController(IRestaurantManagerService restaurantManagerService)
        {
            _restaurantManagerService = restaurantManagerService;
        }

    }
}
