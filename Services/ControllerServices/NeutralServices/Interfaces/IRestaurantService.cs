using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces
{
    public interface IRestaurantService
    {
        Task<JsonResult> GetRestaurantInformationByID(long restaurantID);
        Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request);

        Task<JsonResult> GetRestaurantSummaryByID(long restaurantID);
        Task<JsonResult> GetRestaurantMenuByID(long restaurantID);
    }
}
