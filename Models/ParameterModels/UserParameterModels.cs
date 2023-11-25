namespace ImHungryBackendER.Models.ParameterModels
{
    public class SetCurrentLocationRequest
    {
        public long LocationID { get; set; }
    }

    public class VerifyUsernameRequest
    {
        public string Username { get; set; }
    }

    public class VerifyEmailRequest
    {
        public string Email { get; set; }
    }

    public class VerifyPasswordRequest
    {
        public string PlainPassword { get; set; }
    }

    public class iForgotMyPasswordRequest
    {
        public string Email { get; set; }
        public string PlainPassword { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string EncryptedPassword { get; set; }
    }
}
