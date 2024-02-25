using ImHungryBackendER.Models.ParameterModels;
using Microsoft.AspNetCore.Mvc;

namespace ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces
{
    public interface ICartService
    {
        Task<JsonResult> GetCartInfo();
        Task<JsonResult> GetUserCartItemList();
        Task<int> GetUserCartItemNumber();

        Task AddItemToCart(CartTransactionRequest request);
        Task DecreaseItemAmountByOne(long cartItemID);

        Task<int> getItemAmount(long cartItemID);
        Task<bool> isItemInCartByCartItemId(long cartItemID);
        Task<bool> isItemInCartByParameters(CartTransactionRequest request);
        Task<int> changeAmountOfItem(long cartItemID, int amount);
        Task createItemInCart(CartTransactionRequest request);
        Task removeItemInCart(long cartItemID);
    }
}
