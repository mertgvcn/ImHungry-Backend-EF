namespace WebAPI_Giris.Types
{
    public class Data
    {
        public CartData cartData { get; set; }
        public LocationData locationData { get; set; }
        public UserData userData { get; set; }
        public CreditCardData creditCardData { get; set; }
        public RestaurantData restarantData { get; set; }
    }



    //CART DATA TYPES
    public class CartData
    {
        public List<CartItem>? cartItems { get; set; }
        public int? cartItemNumber { get; set; }
    }

    public class CartItem
    {
        public int restaurantID { get; set; }
        public string name { get; set; }
        public int itemID { get; set; }
        public string itemName { get; set; }
        public string imageSource { get; set; }
        public decimal price { get; set; }
        public int amount { get; set; }
    }



    //LOCATION DATA TYPES
    public class LocationData
    {
        public List<UserLocation> locations { get; set; }
    }  

    public class UserLocation
    {
        public int userID { get; set; } 
        public string province { get; set; }
        public string district { get; set; }
        public string neighbourhood { get; set; }
        public int locationID { get; set; }
        public string street { get; set; }
        public string buildingNo { get; set; }
        public string apartmentNo { get; set; }
        public string note { get; set; }
        public string locationTitle { get; set; }
        public string buildingAddition { get; set; }
    }
    


    //USER DATA TYPES 
    public class UserData
    {
        public AccountInfo accountInfo { get; set; }
        public CurrentLocation currentLocation { get; set; }
    }  

    public class AccountInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }

    public class CurrentLocation
    {
        public string locationTitle { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string neighbourhood { get; set; }
        public string street { get; set; }
        public string buildingNo { get; set; }
        public string buildingAddition { get; set; }
        public string apartmentNo { get; set; }
        public string note { get; set; }
    }



    //CREDIT CARD DATA TYPES
    public class CreditCardData
    {
        public List<UserCreditCard> userCreditCards { get; set; }
    }

    public class UserCreditCard 
    { 
        public int ccID { get; set; }
        public string ccNo { get; set; }
        public string ccName { get; set; }
        public int cvv { get; set; }
        public int userID { get; set; }
        public string expirationDate { get; set; }
    }



    //RESTAURANT DATA TYPES
    public class RestaurantData
    {
        public List<Restaurant> restaurantList { get; set; }
    }

    public class Restaurant
    {
        public int restaurantID { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public string imageSource { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string neighbourhood { get; set; }
        public int locationID { get; set; }
        public string street { get; set; }
        public string buildingNo { get; set; }
    }



}
