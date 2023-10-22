using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models.Parameters.CreditCardParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class CreditCardService : ICreditCardService
    {
        private readonly IUserService userService;
        private readonly IDbService dbService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CreditCardService(IDbService dbService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbService = dbService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonResult> GetUserCreditCards()
        {
            DataTable dataTable = new DataTable();

            int userID = userService.GetCurrentUserID();
            string query = "select * " +
               "from \"User_cc\" " +
               "where \"userID\"=@userID";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
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
                userCreditCards = dataTable
            };

            return new JsonResult(data);
        }

        public async Task<bool> AddCreditCard(AddCreditCardRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = "insert into \"User_cc\" (\"ccNo\",\"ccName\",\"expirationDate\",\"cvv\",\"userID\") " +
                                            "values (@ccNo, @ccName, @expirationDate, @cvv, @userID)";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@ccNo", request.creditCardNumber);
            cmd.Parameters.AddWithValue("@ccName", request.creditCardHolderName);
            cmd.Parameters.AddWithValue("@expirationDate", request.expirationDate);
            cmd.Parameters.AddWithValue("@cvv", request.cvv);

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

        public async Task<bool> DeleteCreditCardByID(int creditCardID)
        {
            string query = "delete from \"User_cc\" where \"ccID\"=@ccID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@ccID", creditCardID);

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
