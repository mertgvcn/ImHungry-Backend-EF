using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Controllers.RestaurantOwnerAPIs
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("RestaurantOwner")]
    public class MenuController : Controller
    {
        [HttpGet("deneme")]
        public void Deneme()
        {

        }
    }
}
