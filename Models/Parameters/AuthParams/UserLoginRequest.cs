namespace WebAPI_Giris.Models.Parameters.AuthParams
{
    public class UserLoginRequest
    {
            public string username { get; set; }
            public string encryptedPassword { get; set; }
    }
}
