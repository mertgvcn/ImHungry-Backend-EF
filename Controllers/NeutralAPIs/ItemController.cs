using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.NeutralAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ItemController : Controller
    {
        private readonly IItemService itemService;

        public ItemController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpPost("GetItemIngredients")]
        public async Task<JsonResult> GetItemIngredients([FromBody] GetItemIngredientRequest request)
        {
            return await itemService.GetItemIngredients(request);
        }
    }
}
