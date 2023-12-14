using ImHungryLibrary.Models;

namespace ImHungryBackendER.Models.ParameterModels
{
    public class GenerateTokenRequest
    {
        public string UserID { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class GenerateTokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}
