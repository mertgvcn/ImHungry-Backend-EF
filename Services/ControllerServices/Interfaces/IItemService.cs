using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IItemService
    {
        Task<JsonResult> GetItemIngredients(GetItemIngredientRequest request); 
    }
}
