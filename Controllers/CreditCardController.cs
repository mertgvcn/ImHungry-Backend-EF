using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.CreditCardParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CreditCardController : Controller
    {
        private readonly ICreditCardService creditCardService;
        private readonly IDbService dbService;

        public CreditCardController(ICreditCardService creditCardService, IDbService dbService)
        {
            this.creditCardService = creditCardService;
            this.dbService = dbService;

            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit; //runs on exit   
        }

        [HttpGet("GetUserCreditCards")]
        public async Task<JsonResult> GetUserCreditCards()
        {
            return await creditCardService.GetUserCreditCards();
        }

        [HttpPost("AddCreditCard")]
        public async Task<bool> AddCreditCard(AddCreditCardRequest request)
        {
            return await creditCardService.AddCreditCard(request);
        }

        [HttpDelete("DeleteCreditCardByID")]
        public async Task<bool> DeleteCreditCardByID([FromQuery] int creditCardID)
        {
            return await creditCardService.DeleteCreditCardByID(creditCardID);
        }

        //Support
        [NonAction]
        private void HandleProcessExit(object sender, EventArgs e)
        {
            dbService.CloseConnection();
        }
    }
}
