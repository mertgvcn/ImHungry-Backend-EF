using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models.Parameters.RestaurantParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly IDbService dbService;

        public RestaurantController(IRestaurantService restaurantService, IDbService dbService)
        {
            this.restaurantService = restaurantService;
            this.dbService = dbService;

            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit;
        }

        [HttpPost("GetRestaurantInfoByID")]
        public async Task<JsonResult> GetRestaurantInfoByID([FromBody] int restaurantID)
        {
            return await restaurantService.GetRestaurantInfoByID(restaurantID);
        }

        [HttpPost("GetRestaurantListByLocation")]
        public async Task<JsonResult> GetRestaurantListByLocation([FromBody] GetRestaurantListByLocationRequest request)
        {
            return await restaurantService.GetRestaurantListByLocation(request);
        }

        //SUPPORT METHODS
        [NonAction]
        private void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }

    }
}
