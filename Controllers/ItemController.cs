using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemService itemService;

        public ItemController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpPost("GetItemIngredients")]
        public async Task<JsonResult> GetItemIngredients([FromBody] int itemID)
        {
            return await itemService.GetItemIngredients(itemID);
        }
    }
}
