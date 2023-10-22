namespace WebAPI_Giris.Models
{
    public class Order
    {

        public int userID { get; set; }

        public int restaurantID { get; set; }

        public int orderID { get; set; }

        public int ccID { get; set; }

        public int locationID { get; set; }

        public DateTime date { get; set; }

        public TimeOnly hour { get; set; }

        public double price { get; set; }

    }
}
