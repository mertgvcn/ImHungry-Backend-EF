using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Diagnostics;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.CartParams;
using WebAPI_Giris.Services.ControllerServices;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;
using WebAPI_Giris.Types;

namespace WebAPI_Giris.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IDbService dbService;

        public CartController(ICartService cartService, IDbService dbService)
        {
            this.cartService = cartService;
            this.dbService = dbService;

            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit; //runs on exit     
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
            return await cartService.AddItemToCart(request);
        }

        [HttpDelete("DeleteItemFromCart")]
        public async Task<bool> DeleteItemFromCart([FromQuery] CartTransactionRequest request)
        {
            if(request.ingredients == "boş") request.ingredients = "";
            return await cartService.DeleteItemFromCart(request);
        }

        //Support
        [NonAction]
        private void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }
    }
}
