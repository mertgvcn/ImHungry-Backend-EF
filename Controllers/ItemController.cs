using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Models.Parameters.ItemParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemService itemService;
        private readonly IDbService dbService;

        public ItemController(IItemService itemService, IDbService dbService)
        {
            this.itemService = itemService;
            this.dbService = dbService;

            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit;
        }

        [HttpPost("GetItemIngredients")]
        public async Task<JsonResult> GetItemIngredients([FromBody] GetItemIngredientsRequest request)
        {
            return await itemService.GetItemIngredients(request.itemID);
        }


        //SUPPORT METHODS
        [NonAction]
        private void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }
    }
}
