using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using System.Security.Claims;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices
{
    public class RestaurantManagerService : IRestaurantManagerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestaurantManagerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Get restaurant id from claim that inside the api token
        public long GetRestaurantID()
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                var restaurantID = _httpContextAccessor.HttpContext.User.FindFirstValue("RestaurantID");
                return int.Parse(restaurantID);
            }

            return -1;
        }
    }
}
