namespace WebAPI_Giris.Models
{
    public class Users
    {
        public int userID { get; set; } 

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public string phoneNumber { get; set; }

        public string password { get; set; }

        public int? locationID { get; set; }

    }
}
