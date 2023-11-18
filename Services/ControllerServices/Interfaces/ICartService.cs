using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ICartService
    {
        public Task<JsonResult> GetCartInfo();
        public Task<JsonResult> GetUserCartItemList();
        public Task<int> GetUserCartItemNumber();

        public Task AddItemToCart(CartTransactionRequest request);
        public Task DecreaseItemAmountByOne(long cartItemID);

        public Task<int> getItemAmount(long cartItemID);
        public Task<bool> isItemExistsOnCart(long cartItemID);
        public Task<int> changeAmountOfItem(long cartItemID, int amount);
        public Task createItemInCart(CartTransactionRequest request);
        public Task removeItemInCart(long cartItemID);
    }
}
