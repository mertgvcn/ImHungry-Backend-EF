using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using static WebAPI_Giris.Services.ControllerServices.Interfaces.ICartService;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class CartService : ICartService
    {
        private readonly ImHungryContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CartService(ImHungryContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
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
            var userID = _userService.GetCurrentUserID();
            var userCartItems = _context.CartItems.Where(a => a.UserId == userID)
                                   .ProjectTo<CartItemViewModel>(_mapper.ConfigurationProvider)
                                   .ToList();

            return new JsonResult(userCartItems);
        }

        public async Task<int> GetUserCartItemNumber()
        {
            var userID = _userService.GetCurrentUserID();
            var userCartItemsAmount = _context.CartItems.Where(a => a.UserId == userID)
                                             .Sum(a => a.Amount);

            return userCartItemsAmount;
        }

        public async Task AddItemToCart(CartTransactionRequest request)
        {
            //item already exist on the cart, so just need to increase amount
            if (await isItemExistsOnCart(request.cartItemID))
            {
                await changeAmountOfItem(request.cartItemID, request.amount);
            }
            //item is not exist in the cart, so create fresh row for that item
            else
            {
                await createItemInCart(request);
            }
        }

        public async Task DecreaseItemAmountByOne(long cartItemID)
        {
            if (await isItemExistsOnCart(cartItemID))
            {
                //if it equals to 0 after decreasing amount of item, remove that item from cart
                if (await changeAmountOfItem(cartItemID, -1) <= 0)
                {
                    await removeItemInCart(cartItemID);
                }
            }
        }

        //Helpers
        public async Task<int> getItemAmount(long cartItemID)
        {
            var userID = _userService.GetCurrentUserID();
            var itemAmount = _context.CartItems
                                .Where(a => a.Id == cartItemID)
                                .Select(a => a.Amount).FirstOrDefault();
                                
            return itemAmount;
        }

        public async Task<bool> isItemExistsOnCart(long cartItemID)
        {
            var itemAmount = await getItemAmount(cartItemID);

            return itemAmount==0 ? false : true;
        }

        public async Task<int> changeAmountOfItem(long cartItemID, int amount)
        {
            var userID = _userService.GetCurrentUserID();
            var cartItem = _context.CartItems
                              .Where(a => a.Id == cartItemID)
                              .FirstOrDefault();

            cartItem.Amount += amount;
            await _context.SaveChangesAsync();
            return cartItem.Amount;
        }

        public async Task createItemInCart(CartTransactionRequest request)
        {
            var userID = _userService.GetCurrentUserID();

            //Following item and restaurant must exist
            var userCart = _context.CartItems.Where(a => a.UserId == userID).ToList();
            var item = _context.Items.Where(a => a.Id == request.itemID).FirstOrDefault();
            var restaurant = _context.Restaurants.Where(a => a.Id == request.restaurantID).FirstOrDefault();

            var newCartItem = new Cart()
            {
                Amount = request.amount,
                IngredientList = request.ingredients,
                UserId = userID,
                Restaurant = restaurant,
                Item = item,
            };

            //Check if userCart has already been created
            if (userCart is null)
                userCart = new List<Cart>() { newCartItem };
            else 
                userCart.Add(newCartItem);

            _context.CartItems.Attach(newCartItem);
            await _context.SaveChangesAsync();
        }

        public async Task removeItemInCart(long cartItemID)
        {
            var cartItem = _context.CartItems.Where(a => a.Id == cartItemID).FirstOrDefault();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
