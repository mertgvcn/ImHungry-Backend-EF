using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using WebAPI_Giris.Models.Parameters.ItemParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class ItemService : IItemService
    {
        private readonly IDbService dbService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ItemService(IDbService dbService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbService = dbService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<JsonResult> GetItemIngredients(int itemID)
        {
            List<GetItemIngredientResponse> ingredients = new List<GetItemIngredientResponse>();

            string query = "select \"ingredientName\" " +
                           "from \"Item_ingredients\" " +
                           "natural join \"Ingredient\" " +
                           "where \"itemID\"=@itemID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@itemID", itemID);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        ingredients.Add(new GetItemIngredientResponse 
                        { 
                            name = (string)reader["ingredientName"], 
                            isActive = true 
                        });

                    }
                }
            }
            catch(Exception ex)
            {
                httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(ex.StackTrace);
            }

            return new JsonResult(ingredients);
        }
    }
}
