using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models.Parameters.RestaurantParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IDbService dbService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RestaurantService(IDbService dbService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbService = dbService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonResult> GetRestaurantInfoByID(int restaurantID)
        {
            var restaurantDetails = await GetRestaurantSummaryByID(restaurantID);
            var menu = await GetRestaurantMenuByID(restaurantID);

            var restaurantInfo = new
            {
                restaurantDetails = restaurantDetails.Value,
                menu = menu.Value
            };

            return new JsonResult(restaurantInfo);
        }

        public async Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request)
        {
            DataTable dataTable = new DataTable();

            string query = "select * " +
               "from \"Restaurant\" " +
               "natural join \"Restaurant_location\" " +
               "where lower(\"province\") like lower(@province) " +
               "  and lower(\"district\") like lower(@district);";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@province", '%' + request.province + '%');
            cmd.Parameters.AddWithValue("@district", '%' + request.district + '%');

            try
            {
                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    dataTable.Load(reader);
                }
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            var data = new
            {
                restaurantList = dataTable
            };

            return new JsonResult(data);
        }

        public async Task<JsonResult> GetRestaurantSummaryByID(int restaurantID)
        {
            JsonResult jsonResult = new JsonResult(null);

            string query = "select * from \"Restaurant\" where \"restaurantID\"=@restaurantID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@restaurantID", restaurantID);

            try
            {
                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var restaurantDetails = new
                        {
                            restaurantID = (Int32)reader["restaurantID"],
                            name = (string)reader["name"],
                            phoneNumber = (string)reader["phoneNumber"],
                            email = (string)reader["email"],
                            description = (string)reader["description"],
                            imageSource = (string)reader["imageSource"],
                        };

                        jsonResult = new JsonResult(restaurantDetails);
                    }
                }
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            return jsonResult;
        }

        public async Task<JsonResult> GetRestaurantMenuByID(int restaurantID)
        {
            DataTable dataTable = new DataTable();

            string query = "select * " +
                "from \"Menu\" " +
                "natural join \"Item\" " +
                "natural join \"Category\" " +
                "where \"restaurantID\"=@restaurantID " +
                "order by \"categoryID\";";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@restaurantID", restaurantID);

            try
            {
                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            return new JsonResult(dataTable);
        }
    }
}
