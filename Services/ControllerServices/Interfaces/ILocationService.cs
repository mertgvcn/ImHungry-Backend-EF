using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Models.Parameters.LocationParams;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ILocationService
    {
        public Task<JsonResult> GetUserLocationList();

        public Task<bool> AddLocation(AddLocationRequest request);
        public Task<bool> DeleteLocationByLocationID(int locationID);
    }
}
