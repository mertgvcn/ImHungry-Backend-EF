using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using ImHungryLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices
{
    public class MenuService : IMenuServices
    {
        private readonly ImHungryContext _context;
        private readonly IMapper _mapper;
        private readonly IRestaurantManagerService _restaurantManagerService;
        private readonly IRestaurantService _restaurantService;

        public MenuService(
            ImHungryContext context,
            IMapper mapper,
            IRestaurantManagerService restaurantManagerService, 
            IRestaurantService restaurantService)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<JsonResult> GetCategories()
        {
            var restaurantID = _restaurantManagerService.GetRestaurantID();
            var categories = _context.Categories
                                .Where(x=> x.RestaurantId == restaurantID)
                                .ToList();

            return new JsonResult(categories);
        }

        public async Task AddCategory(AddCategoryRequest request)
        {
            var restaurantId = _restaurantManagerService.GetRestaurantID();

            var newCategory = new Category()
            {
                Name = request.Name,
                isActive = true,
                RestaurantId = restaurantId,   
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryById(long categorId)
        {
            var category = _context.Categories.Where(a => a.Id == categorId).FirstOrDefault();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
