using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
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
        public async Task<bool> AddItemToCart([FromBody] CartTransactionRequest request)
        {
            if (request.ingredients == "") request.ingredients = null;

            return await cartService.AddItemToCart(request);
        }

        [HttpDelete("DeleteItemFromCart")]
        public async Task<bool> DeleteItemFromCart([FromQuery] CartTransactionRequest request, [FromQuery] long cartItemID)
        {
            if(request.ingredients == "") request.ingredients = null;

            return await cartService.DeleteItemFromCart(request, cartItemID);
        }
    }
}
