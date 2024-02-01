namespace ImHungryBackendER.Models.ParameterModels
{
    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
    }

    public class UserLoginResponse
    {
        public bool AuthenticateResult { get; set; }
        public string AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public string Role { get; set; }
    }

    public class UserRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterResponse
    {
        public bool isSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
