using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.RestaurantParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;
using WebAPI_Giris.Types;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PageContentController : Controller
    {
        private readonly IDbService dbService;
        private readonly ICryptionService cryptionService;
        private readonly IUserService userService;
        private readonly IRestaurantService restaurantService;
        private readonly ILocationService locationService;
        private readonly ICreditCardService creditCardService;
        private readonly ICartService cartService;

        public PageContentController(IDbService dbService, 
            ICryptionService cryptionService, 
            IUserService userService,
            IRestaurantService restaurantService,
            ILocationService locationService,
            ICreditCardService creditCardService,
            ICartService cartService)
        {
            this.dbService = dbService;
            this.cryptionService = cryptionService;
            this.userService = userService;
            this.restaurantService = restaurantService;
            this.locationService = locationService;
            this.creditCardService = creditCardService;
            this.cartService = cartService;
        }

        //Home Page Data
        [HttpGet("HomePageData")]
        public async Task<JsonResult> HomePageData()
        {
            UserController u = new UserController(userService, dbService);
            RestaurantController r = new RestaurantController(restaurantService, dbService);

            JsonResult user = await u.GetUserInfo();
            JsonResult restaurant = new JsonResult(null);


            //Fetch restaurants if user have location, could be done by another hasLocation request. But its a serialize,deserialize example.
            var serializedUserData = JsonConvert.SerializeObject(user.Value);
            UserData userData = JsonConvert.DeserializeObject<UserData>(serializedUserData);
            if (userData.currentLocation != null)
            {
                GetRestaurantListByLocationRequest request = new();
                request.province = userData.currentLocation.province;
                request.district = userData.currentLocation.district;

                restaurant = await r.GetRestaurantListByLocation(request);
            }


            //Preparing final form of json result
            var data = new
            {
                currentLocation = userData.currentLocation,
                restaurant = restaurant.Value,
            };


            return new JsonResult(data);
        }

        //Profile Page Data
        [HttpGet("ProfilePageData")]
        public async Task<JsonResult> ProfilePageData()
        {
            UserController u = new UserController(userService, dbService);
            LocationController l = new LocationController(locationService, dbService);
            CreditCardController c = new CreditCardController(creditCardService, dbService);

            JsonResult user = await u.GetAccountInfo();
            JsonResult location = await l.GetUserLocationList();
            JsonResult creditCard = await c.GetUserCreditCards();


            //Preparing final form of json result
            var data = new
            {
                accountInfo = user.Value,
                location = location.Value,
                creditCard = creditCard.Value,
            };


            return new JsonResult(data);
        }

        //Navbar Data
        [HttpGet("NavbarData")]
        public async Task<JsonResult> NavbarData()
        {
            CartController c = new CartController(cartService, dbService);

            JsonResult cart = await c.GetCartInfo();

            //Preparing final form of json result
            var data = new
            {
                cart = cart.Value,
            };


            return new JsonResult(data);
        }

        //Restaurant Details Data
        [HttpGet("RestaurantDetailsPageData")]
        public async Task<JsonResult> RestaurantDetailsPageData([FromQuery] int restaurantID)
        {
            RestaurantController r = new RestaurantController(restaurantService, dbService);

            JsonResult restaurant = await r.GetRestaurantInfoByID(restaurantID);

            //Preparing final form of json result
            var data = new
            {
                restaurant = restaurant.Value,
            };


            return new JsonResult(data);
        }
    }
}
