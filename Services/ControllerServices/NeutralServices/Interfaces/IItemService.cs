using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces
{
    public interface IItemService
    {
        Task<JsonResult> GetItemIngredients(GetItemIngredientRequest request);
    }
}
