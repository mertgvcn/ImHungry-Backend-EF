using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices
{
    public class MenuService : IMenuService
    {
        private readonly IRestaurantManagerService _restaurantManagerService;
        private readonly IRestaurantService _restaurantService;

        public MenuService(
            IRestaurantManagerService restaurantManagerService, 
            IRestaurantService restaurantService)
        {
            _restaurantManagerService = restaurantManagerService;
            _restaurantService = restaurantService;
        }

        public async Task<JsonResult> GetMenu()
        {
            var restaurantID = _restaurantManagerService.GetRestaurantID();
            var menu = await _restaurantService.GetRestaurantMenuByID(restaurantID);

            return menu;
        } 

        public async Task AddItemToMenu()
        {

        }
    }
}
