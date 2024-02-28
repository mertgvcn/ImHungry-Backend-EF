using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces
{
    public interface IMenuService
    {
        public Task<JsonResult> GetMenu();
        public Task AddItemToMenu();
    }
}
