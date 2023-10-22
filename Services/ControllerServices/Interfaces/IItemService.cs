using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IItemService
    {
        public Task<JsonResult> GetItemIngredients(int itemID); 
    }
}
