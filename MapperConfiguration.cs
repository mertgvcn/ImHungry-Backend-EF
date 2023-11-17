using AutoMapper;
using ImHungryBackendER.Models.ViewModels;
using TypeSupport.Extensions;
using WebAPI_Giris.Models;

namespace ImHungryBackendER
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<User, UserAccountViewModel>();
            CreateMap<UserLocation, LocationViewModel>();

            CreateMap<Cart, CartItemViewModel>();
            CreateMap<Restaurant, CartItem_RestaurantViewModel>();
            CreateMap<Item, CartItem_ItemViewModel>();

            CreateMap<Restaurant, RestaurantListViewModel>();
            CreateMap<Restaurant, RestaurantSummaryViewModel>();

            CreateMap<Item, ItemViewModel>();

            CreateMap<CreditCard, CreditCardViewModel>();
            //CreateMap<User, AccountViewModel>().ForMember(a => a.ccCount, b => b.MapFrom(c => c.CreditCards.Count));
        }
    }
}
