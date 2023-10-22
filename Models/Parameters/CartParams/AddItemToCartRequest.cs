namespace WebAPI_Giris.Models.Parameters.CartParams
{
    public class AddItemToCartRequest
    {
        public int itemID { get; set; }
        public int restaurantID { get; set; }
        public string ingredients { get; set; }
        public int amount { get; set; }
    }
}
