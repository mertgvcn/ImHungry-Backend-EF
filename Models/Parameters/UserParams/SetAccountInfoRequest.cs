namespace WebAPI_Giris.Models.Parameters.UserParams
{
    public class SetAccountInfoRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
}
