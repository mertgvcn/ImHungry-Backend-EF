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
            var userCartItems = _context.Users.Where(a => a.Id == userID)
                                   .Include(a => a.CartItems).ThenInclude(a => a.Item).ThenInclude(a => a.Category)
                                   .Include(a => a.CartItems).ThenInclude(a => a.Restaurant)
                                   .FirstOrDefault()!.CartItems.AsQueryable()
                                   .ProjectTo<CartItemViewModel>(_mapper.ConfigurationProvider)
                                   .ToList();

            return new JsonResult(userCartItems);
        }

        public async Task<int> GetUserCartItemNumber()
        {
            var userID = _userService.GetCurrentUserID();
            var userCartItemsAmountList = _context.Users.Where(a => a.Id == userID)
                                             .Include(a => a.CartItems).FirstOrDefault()!.CartItems.AsQueryable()
                                             .Select(a => a.Amount)
                                             .ToList();

            int itemNumber = 0;
            userCartItemsAmountList.ForEach(a => { itemNumber += a; });

            return itemNumber;
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

        public async Task<bool> DeleteItemFromCart(CartTransactionRequest request, long cartItemID)
        {
            await changeAmountOfItem(request);

            //if it equals to 0 after decreasing amount of item, remove that item from cart
            if (await getItemAmount(request) == 0)
            {
                return await removeItemInCart(cartItemID);
            }

            return true;
        }


        //Helpers
        public async Task<int> getItemAmount(CartTransactionRequest request)
        {
            var userID = _userService.GetCurrentUserID();
            var userCart = _context.Users.Where(a => a.Id == userID)
                              .Include(a => a.CartItems).ThenInclude(a => a.Restaurant)
                              .Include(a => a.CartItems).ThenInclude(a => a.Item)
                              .Include(a => a.CartItems)
                              .FirstOrDefault()!.CartItems.AsQueryable();

            var itemAmount = userCart
                                .Where(a => a.Item.Id == request.itemID)
                                .Where(a => a.Restaurant.Id == request.restaurantID)
                                .Where(a => a.IngredientList == request.ingredients)
                                .Select(a => a.Amount).FirstOrDefault();
                              
            return itemAmount;
        }

        public async Task<bool> isItemExistsOnCart(CartTransactionRequest request)
        {
            var userID = _userService.GetCurrentUserID();
            var userCart = _context.Users.Where(a => a.Id == userID)
                              .Include(a => a.CartItems).ThenInclude(a => a.Restaurant)
                              .Include(a => a.CartItems).ThenInclude(a => a.Item)
                              .Include(a => a.CartItems)
                              .FirstOrDefault()!.CartItems.AsQueryable();

            var cartItemID = userCart
                                .Where(a => a.Item.Id == request.itemID)
                                .Where(a => a.Restaurant.Id == request.restaurantID)
                                .Where(a => a.IngredientList == request.ingredients)
                                .Select(a => a.Id).FirstOrDefault();

            return cartItemID==0 ? false : true;
        }

        public async Task<bool> changeAmountOfItem(CartTransactionRequest request)
        {
            var userID = _userService.GetCurrentUserID();
            var userCart = _context.Users.Where(a => a.Id == userID)
                              .Include(a => a.CartItems).ThenInclude(a => a.Restaurant)
                              .Include(a => a.CartItems).ThenInclude(a => a.Item)
                              .Include(a => a.CartItems)
                              .FirstOrDefault()!.CartItems.AsQueryable();

            var cartItem = userCart
                                .Where(a => a.Item.Id == request.itemID)
                                .Where(a => a.Restaurant.Id == request.restaurantID)
                                .Where(a => a.IngredientList == request.ingredients)
                                .FirstOrDefault();

            cartItem.Amount = (int)(cartItem.Amount + request.amount);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> createItemInCart(CartTransactionRequest request)
        {
            var userID = _userService.GetCurrentUserID();
            var userCart = _context.Users.Where(a => a.Id == userID)
                              .Include(a => a.CartItems)
                              .FirstOrDefault()!.CartItems;

            //Following item and restaurant must exist
            var item = _context.Items.Where(a => a.Id == request.itemID).FirstOrDefault();
            var restaurant = _context.Restaurants.Where(a => a.Id == request.restaurantID).FirstOrDefault();

            var newCartItem = new Cart()
            {
                Amount = (int)request.amount,
                IngredientList = request.ingredients,
                Restaurant = restaurant,
                Item = item
            };

            //Check if userCart has already been created
            if (userCart is null)
                userCart = new List<Cart>() { newCartItem };
            else 
                userCart.Add(newCartItem);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> removeItemInCart(long cartItemID)
        {
            var cartItem = _context.CartItems.Where(a => a.Id == cartItemID).FirstOrDefault();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
