namespace WebAPI_Giris.Models
{
    public class Cart
    {
        public int cartItemID { get; set; }

        public int userID { get; set; }

        public int restaurantID { get; set; }

        public int itemID { get; set; }

        public string ingredients { get; set; }
    }
}
