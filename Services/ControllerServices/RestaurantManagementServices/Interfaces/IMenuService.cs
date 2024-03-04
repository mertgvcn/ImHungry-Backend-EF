using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces
{
    public interface IMenuService
    {
        public Task<JsonResult> GetMenu();
        public Task<JsonResult> GetCategories();

        public Task AddCategory(AddCategoryRequest request);
        public Task AddItemToMenu();
    }
}
