using Microsoft.AspNetCore.Mvc;
using WebAPI_Giris.Models.Parameters.RestaurantParams;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface IRestaurantService
    {
        public Task<JsonResult> GetRestaurantInfoByID(int restaurantID);
        public Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request);

        public Task<JsonResult> GetRestaurantSummaryByID(int restaurantID);
        public Task<JsonResult> GetRestaurantMenuByID(int restaurantID);
    }
}
