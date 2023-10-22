using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.CartParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;
using WebAPI_Giris.Types;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class CartService : ICartService
    {
        private readonly IUserService userService;
        private readonly IDbService dbService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CartService(IUserService userService, IDbService dbService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.dbService = dbService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Get general cart info
        public async Task<JsonResult> GetCartInfo()
        {
            var cartItems = await GetUserCartItemList();
            var cartItemNumber = await GetUserCartItemNumber();

            var cartInfo = new
            {
                cartItems = cartItems.Value,
                cartItemNumber = cartItemNumber
            };

            return new JsonResult(cartInfo);
        }

        public async Task<JsonResult> GetUserCartItemList()
        {
            DataTable dataTable = new DataTable();

            int userID = userService.GetCurrentUserID();
            string query = "select re.\"restaurantID\",re.\"name\",it.\"itemID\",it.\"itemName\",it.\"imageSource\",it.\"price\",ca.\"amount\",ca.\"ingredients\" " +
                "from \"Cart\" ca " +
                "join \"Restaurant\" re on re.\"restaurantID\"=ca.\"restaurantID\" " +
                "join \"Item\" it on it.\"itemID\"=ca.\"itemID\" " +
                "where \"userID\"=@userID " +
                "group by re.\"restaurantID\", it.\"itemID\", ca.\"cartItemID\" " +
                "order by it.\"price\";";

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

            return new JsonResult(dataTable);
        }

        public async Task<int> GetUserCartItemNumber()
        {
            Int64 itemNumber = 0;

            int userID = userService.GetCurrentUserID();
            string query = "select coalesce(sum(\"amount\"), 0) as \"count\" " + //coalesce is a null handler. If sum("amount") returns null, it assigns 0 to value. 
                           "from \"Cart\" " +
                           "where \"userID\"=@userID;";


            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        itemNumber = Convert.ToInt64(reader["count"]);
                    }
                }
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            return (int)itemNumber;
        }

        public async Task<bool> AddItemToCart(CartTransactionRequest request)
        {
            //item already exist on the cart, so just need to increase amount
            if (await isItemExistsOnCart(request))
            {
                return await changeAmountOfItem(request);
            }
            //item is not exist in the cart, so create fresh row for that item
            else
            {
                return await createItemInCart(request);
            }
        }

        public async Task<bool> DeleteItemFromCart(CartTransactionRequest request)
        {
            await changeAmountOfItem(request);

            //if it equals to 0 after decreasing amount of item, remove that item from cart
            if (await getItemAmount(request) == 0)
            {
                return await removeItemInCart(request);
            }

            return true;
        }



        //Helpers
        public async Task<int> getItemAmount(CartTransactionRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = $"select \"amount\" " +
               $"from \"Cart\" " +
               $"where \"userID\"=@userID and \"itemID\"=@itemID and \"restaurantID\"=@restaurantID and \"ingredients\"=@ingredients;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@itemID", request.itemID);
            cmd.Parameters.AddWithValue("@restaurantID", request.restaurantID);
            cmd.Parameters.AddWithValue("@ingredients", request.ingredients);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int amount = (int)reader["amount"];
                        return amount;
                    }

                    return -1;
                }
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
            finally
            {
                dbService.CloseConnection();
            }
        }

        public async Task<bool> isItemExistsOnCart(CartTransactionRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = "select \"cartItemID\" " +
               "from \"Cart\" " +
               "where \"userID\"=@userID and \"restaurantID\"=@restaurantID and \"itemID\"=@itemID and \"ingredients\"=@ingredients;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@itemID", request.itemID);
            cmd.Parameters.AddWithValue("@restaurantID", request.restaurantID);
            cmd.Parameters.AddWithValue("@ingredients", request.ingredients);

            try
            {
                bool key = false;

                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        key = true;
                    }
                }

                return key;
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }

        public async Task<bool> changeAmountOfItem(CartTransactionRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = "update \"Cart\" " +
                    "set \"amount\"=\"amount\"+# " +
                    "where \"userID\"=@userID and \"itemID\"=@itemID and \"restaurantID\"=@restaurantID and \"ingredients\"=@ingredients;";

            /*
            if (isIncreasing)
            {
                query = query.Replace("#", "+1");
            }
            else
            {
                query = query.Replace("#", "-1");
            }
            */

            query = query.Replace("#", request.amount.ToString());

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@itemID", request.itemID);
            cmd.Parameters.AddWithValue("@restaurantID", request.restaurantID);
            cmd.Parameters.AddWithValue("@ingredients", request.ingredients);

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

        public async Task<bool> createItemInCart(CartTransactionRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = $"insert into \"Cart\" (\"userID\",\"itemID\",\"restaurantID\",\"ingredients\",\"amount\") " +
                  $"values (@userID, @itemID, @restaurantID, @ingredients, @amount);";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@itemID", request.itemID);
            cmd.Parameters.AddWithValue("@restaurantID", request.restaurantID);
            cmd.Parameters.AddWithValue("@ingredients", request.ingredients);
            cmd.Parameters.AddWithValue("@amount", request.amount);

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

        public async Task<bool> removeItemInCart(CartTransactionRequest request)
        {
            int userID = userService.GetCurrentUserID();
            string query = $"delete from \"Cart\" " +
                           $"where \"userID\"=@userID and \"itemID\"=@itemID and \"restaurantID\"=@restaurantID and \"ingredients\"=@ingredients;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@itemID", request.itemID);
            cmd.Parameters.AddWithValue("@restaurantID", request.restaurantID);
            cmd.Parameters.AddWithValue("@ingredients", request.ingredients);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                dbService.CloseConnection();
                return true;
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new System.Exception(e.StackTrace);
            }

        }

        /*
        public async Task<bool> isSameRestaurant(int restaurantID) //check if item gonna be added belongs to another restaurant
        {
            int userID = userService.GetCurrentUserID();
            string query = "select \"restaurantID\" " +
               "from \"Cart\" " +
               "where \"userID\"=@userID " +
               "group by \"restaurantID\";";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                bool isSame = false;

                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {

                    while (await reader.ReadAsync())
                    {
                        int storedRestaurantID = (int)reader["restaurantID"];

                        if (restaurantID == storedRestaurantID) //Return true if the ID from the parameter is the same as the restaurant ID that read.
                        {
                            isSame = true;
                        }
                    }
                }

                //sepet boş
                return isSame;
            }
            catch (Exception e)
            {
                this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);    
            }
        }
        */
    }
}
