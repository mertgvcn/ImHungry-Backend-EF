using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ILocationService
    {
        Task<JsonResult> GetUserLocationList();

        Task AddLocation(AddLocationRequest request);
        Task DeleteLocationByLocationID(long locationID);
    }
}
