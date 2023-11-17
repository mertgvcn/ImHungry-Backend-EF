using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ICreditCardService
    {
        public Task<JsonResult> GetUserCreditCards();
        public Task AddCreditCard(AddCreditCardRequest request);
        public Task DeleteCreditCardByID(long creditCardID);
    }
}
