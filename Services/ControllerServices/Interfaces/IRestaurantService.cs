using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IRestaurantService
    {
        Task<JsonResult> GetRestaurantInformationByID(int restaurantID);
        Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request);
        
        Task<JsonResult> GetRestaurantSummaryByID(int restaurantID);
        Task<JsonResult> GetRestaurantMenuByID(int restaurantID);
    }
}
