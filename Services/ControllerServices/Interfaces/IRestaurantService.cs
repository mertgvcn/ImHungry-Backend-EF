using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IRestaurantService
    {
        public Task<JsonResult> GetRestaurantInformationByID(int restaurantID);
        public Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request);
        
        public Task<JsonResult> GetRestaurantSummaryByID(int restaurantID);
        public Task<JsonResult> GetRestaurantMenuByID(int restaurantID);
    }
}
