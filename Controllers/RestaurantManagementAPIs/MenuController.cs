using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.RestaurantOwnerAPIs
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "RestaurantOwner")]
    public class MenuController : Controller
    {
        private readonly IMenuServices _menuService;

        public MenuController(IMenuServices menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("GetMenu")]
        public async Task<JsonResult> GetMenu()
        {
            return await _menuService.GetMenu();
        }

        [HttpGet("GetCategories")]
        public async Task<JsonResult> GetCategories()
        {
            return await _menuService.GetCategories();
        }

        [HttpPost("AddCategory")]
        public async Task AddCategory([FromBody] AddCategoryRequest request)
        {
            await _menuService.AddCategory(request);
        }

        [HttpDelete("DeleteCategoryById")]
        public async Task DeleteCategoryById([FromQuery] long categoryId)
        {
            await _menuService.DeleteCategoryById(categoryId);
        }
    }
}
