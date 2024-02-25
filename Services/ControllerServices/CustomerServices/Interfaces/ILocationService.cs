using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces
{
    public interface ILocationService
    {
        Task<JsonResult> GetUserLocationList();

        Task AddLocation(AddLocationRequest request);
        Task DeleteLocationByLocationID(long locationID);
    }
}
