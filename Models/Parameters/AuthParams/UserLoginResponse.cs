namespace WebAPI_Giris.Models.Parameters.AuthParams
{
    public class UserLoginResponse
    {
        public bool authenticateResult { get; set; }
        public string authToken { get; set; }
        public DateTime accessTokenExpireDate { get; set; }
    }
}
