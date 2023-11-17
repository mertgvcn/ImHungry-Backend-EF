using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ILocationService
    {
        public Task<JsonResult> GetUserLocationList();

        public Task AddLocation(AddLocationRequest request);
        public Task DeleteLocationByLocationID(long locationID);
    }
}
