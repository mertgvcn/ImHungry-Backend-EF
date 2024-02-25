using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces
{
    public interface ICreditCardService
    {
        Task<JsonResult> GetUserCreditCards();
        Task AddCreditCard(AddCreditCardRequest request);
        Task DeleteCreditCardByID(long creditCardID);
    }
}
