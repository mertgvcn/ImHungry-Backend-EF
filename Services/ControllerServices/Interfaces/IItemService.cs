using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IItemService
    {
        public Task<JsonResult> GetItemIngredients(GetItemIngredientRequest request); 
    }
}
