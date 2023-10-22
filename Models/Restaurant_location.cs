namespace WebAPI_Giris.Models
{
    public class Restaurant_location
    {
        public int restaurantID { get; set; }

        public string province { get; set; }

        public string district { get; set; }

        public string neighbourhood { get; set; }

        public string street { get; set; }

        public string buildingNo { get; set; }

        public string? apartmentNo { get; set; }

        public string? addition { get; set; }

        public int locationID { get; set; }

    }
}
