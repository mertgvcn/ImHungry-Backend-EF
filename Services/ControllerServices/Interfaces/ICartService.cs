using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Giris.Services.ControllerServices.Interfaces
{
    public interface ICartService
    {
        public Task<JsonResult> GetCartInfo();
        public Task<JsonResult> GetUserCartItemList();
        public Task<int> GetUserCartItemNumber();
        public Task<bool> AddItemToCart(CartTransactionRequest request);
        public Task<bool> DeleteItemFromCart(CartTransactionRequest request, long cartItemID);

        public Task<int> getItemAmount(CartTransactionRequest request);
        public Task<bool> isItemExistsOnCart(CartTransactionRequest request);
        public Task<bool> changeAmountOfItem(CartTransactionRequest request);
        public Task<bool> createItemInCart(CartTransactionRequest request);
        public Task<bool> removeItemInCart(long cartItemID);
    }
}
