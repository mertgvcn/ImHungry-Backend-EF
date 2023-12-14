using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CreditCardController : Controller
    {
        private readonly ICreditCardService creditCardService;

        public CreditCardController(ICreditCardService creditCardService)
        {
            this.creditCardService = creditCardService;
        }

        [HttpGet("GetUserCreditCards")]
        public async Task<JsonResult> GetUserCreditCards()
        {
            return await creditCardService.GetUserCreditCards();
        }

        [HttpPost("AddCreditCard")]
        public async Task AddCreditCard([FromBody] AddCreditCardRequest request)
        {
            await creditCardService.AddCreditCard(request);
        }

        [HttpDelete("DeleteCreditCardByID")]
        public async Task DeleteCreditCardByID([FromQuery] long creditCardID)
        {
            await creditCardService.DeleteCreditCardByID(creditCardID);
        }
    }
}
