using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ICreditCardService
    {
        Task<JsonResult> GetUserCreditCards();
        Task AddCreditCard(AddCreditCardRequest request);
        Task DeleteCreditCardByID(long creditCardID);
    }
}
