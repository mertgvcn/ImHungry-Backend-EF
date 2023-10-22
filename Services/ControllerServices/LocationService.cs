using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.LocationParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class LocationService : ILocationService
    {
        private readonly IDbService dbService;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocationService(IDbService dbService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbService = dbService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonResult> GetUserLocationList()
        {
            DataTable dataTable = new DataTable();

            int userID = userService.GetCurrentUserID();
            string query = "select * " +
               "from \"User_location\" " +
               "where \"userID\"=@userID";


            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

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

            var data = new
            {
                userLocations = dataTable
            };

            return new JsonResult(data);
        }

        public async Task<bool> AddLocation(AddLocationRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = $"insert into \"User_location\" (\"userID\",\"locationTitle\",\"province\",\"district\",\"neighbourhood\",\"street\",\"buildingNo\",\"buildingAddition\",\"apartmentNo\",\"note\") " +
                                                  $"values (@userID, @locationTitle, @province, @district, @neighbourhood, @street, @buildingNo, @buildingAddition, @apartmentNo, @note);";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@locationTitle", request.locationTitle);
            cmd.Parameters.AddWithValue("@province", request.province);
            cmd.Parameters.AddWithValue("@district", request.district);
            cmd.Parameters.AddWithValue("@neighbourhood", request.neighbourhood);
            cmd.Parameters.AddWithValue("@street", request.street);
            cmd.Parameters.AddWithValue("@buildingNo", request.buildingNo);
            cmd.Parameters.AddWithValue("@buildingAddition", request.buildingAddition);
            cmd.Parameters.AddWithValue("@apartmentNo", request.apartmentNo);
            cmd.Parameters.AddWithValue("@note", request.note);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }

        public async Task<bool> DeleteLocationByLocationID(int locationID)
        {
            string query = "delete from \"User_location\" where \"locationID\"=@locationID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@locationID", locationID);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }
    }
}
