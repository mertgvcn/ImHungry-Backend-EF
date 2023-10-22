namespace WebAPI_Giris.Models.Parameters.TokenParams
{
    public class GenerateTokenResponse
    {
        public string token { get; set; }
        public DateTime tokenExpireDate { get; set; }
    }
}
