using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.UserAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("GetCartInfo")]
        public async Task<JsonResult> GetCartInfo()
        {
            return await cartService.GetCartInfo();
        }

        [HttpGet("GetUserCartItemList")]
        public async Task<JsonResult> GetUserCartItemList()
        {
            return await cartService.GetUserCartItemList();
        }

        [HttpGet("GetUserCartItemNumber")]
        public async Task<int> GetUserCartItemNumber()
        {
            return await cartService.GetUserCartItemNumber();
        }

        [HttpPost("AddItemToCart")]
        public async Task AddItemToCart([FromBody] CartTransactionRequest request)
        {
            if (request.Ingredients == "") request.Ingredients = null;

            await cartService.AddItemToCart(request);
        }

        [HttpDelete("DecreaseItemAmountByOne")]
        public async Task DecreaseItemAmountByOne([FromQuery] long cartItemID)
        {
            await cartService.DecreaseItemAmountByOne(cartItemID);
        }
    }
}
