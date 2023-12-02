using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class ItemService : IItemService
    {
        private readonly ImHungryContext _context;

        public ItemService(ImHungryContext context)
        {
            _context = context;
        }

        public async Task<JsonResult> GetItemIngredients(GetItemIngredientRequest request)
        {
            List<GetItemIngredientResponse> ingredients = new List<GetItemIngredientResponse>();

            var ingredientNameList = _context.Items
                                        .Where(item => item.Id == request.ItemId)
                                        .Include(a => a.Ingredients).FirstOrDefault()!.Ingredients.AsQueryable()
                                        .Select(a => a.Name)
                                        .ToList();

            ingredientNameList.ForEach( ingredientName =>
            {
                ingredients.Add(
                    new GetItemIngredientResponse
                    {
                        Name = ingredientName,
                        isActive = true
                    }
                );
            });

            return new JsonResult(ingredients);
        }
    }
}
