namespace WebAPI_Giris.Models
{
    public class User_location
    {
        public int userID { get; set; }

        public string locationTitle { get; set; }

        public string province { get; set; }

        public string district { get; set; }

        public string neighbourhood { get; set; }

        public string? street { get; set; }

        public string? buildingNo { get; set; }

        public string? buildingAddition { get; set; }

        public string? apartmentNo { get; set; }

        public string? note { get; set; }

        public int locationID { get; set; }

    }
}
