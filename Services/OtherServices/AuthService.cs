using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using WebAPI_Giris.Models;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class AuthService : IAuthService
    {
        private readonly ImHungryContext _context;
        private readonly ICryptionService cryptionService;
        private readonly ITokenService tokenService;
        private readonly IUserService userService;

        public AuthService(
            ImHungryContext context,
            ICryptionService cryptionService, 
            ITokenService tokenService, 
            IUserService userService)
        {
            _context = context;
            this.cryptionService = cryptionService;
            this.tokenService = tokenService;
            this.userService = userService;
        }

        public async Task<long> GetIdByUsername(string Username)
        {
            var user = _context.Users.Where(user => user.Username == Username).FirstOrDefault();
            if (user is null) return -1;

            return user.Id;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new();
            bool isLogin = false;
            string plainPassword = cryptionService.Decrypt(request.EncryptedPassword);

            var user = _context.Users.Where(user => user.Username == request.Username).FirstOrDefault();
            if (user is null) return response;

            isLogin = BCrypt.Net.BCrypt.Verify(request.EncryptedPassword, user.Password); //request.Encryptedpass -> plainpassword çevir

            if (isLogin)
            {
                var generatedToken = await tokenService.GenerateToken(new GenerateTokenRequest { UserID = user.Id.ToString() });

                response.AuthenticateResult = true;
                response.AuthToken = generatedToken.Token;
                response.AccessTokenExpireDate = generatedToken.TokenExpireDate;
            }

            return await Task.FromResult(response);
        }

        public async Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest user)
        {
            UserRegisterResponse response = new();
            response.isSuccess = true;

            //check if username already exists
            if (await userService.VerifyUsername(user.Username))
            {
                response.isSuccess = false;
                response.ErrorMessage = "Invalid username";
                return response;
            }

            string password = cryptionService.Decrypt(user.Password); //The password comes encrypted from the frontend
            password = BCrypt.Net.BCrypt.HashPassword(password); //Hashing the password for security

            User newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return await Task.FromResult(response);
        }
    }
}
