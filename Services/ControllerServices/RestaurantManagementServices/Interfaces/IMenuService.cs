using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces
{
    public interface IMenuServices
    {
        public Task<JsonResult> GetMenu();
        public Task<JsonResult> GetCategories();

        public Task AddCategory(AddCategoryRequest request);
        public Task AddItemToMenu();

        public Task DeleteCategoryById(long categoryId);
    }
}
