using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.CreditCardParams;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ICreditCardService
    {
        public Task<JsonResult> GetUserCreditCards();
        public Task<bool> AddCreditCard(AddCreditCardRequest request);
        public Task<bool> DeleteCreditCardByID(int ccID);
    }
}
