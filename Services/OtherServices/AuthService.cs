using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryLibrary.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class AuthService : IAuthService
    {
        private readonly ImHungryContext _context;
        private readonly ICryptionService _cryptionService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService(
            ImHungryContext context,
            ICryptionService cryptionService, 
            ITokenService tokenService, 
            IUserService userService)
        {
            _context = context;
            _cryptionService = cryptionService;
            _tokenService = tokenService;
            _userService = userService;
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
            string plainPassword = _cryptionService.Decrypt(request.EncryptedPassword);

            var user = _context.Users.Where(user => user.Username == request.Username)
                            .Include(a => a.Roles).FirstOrDefault();
            if (user is null) return response;

            isLogin = BCrypt.Net.BCrypt.Verify(plainPassword, user.Password);

            if (isLogin)
            {
                var generatedToken = await _tokenService.GenerateToken(new GenerateTokenRequest { 
                    UserID = user.Id.ToString(),
                    Roles = user.Roles.ToList(),
                });

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

            //Check if username already exists
            if (await _userService.VerifyUsername(new VerifyUsernameRequest { Username = user.Username } ))
            {
                response.isSuccess = false;
                response.ErrorMessage = "Invalid username";
                return response;
            }

            string password = _cryptionService.Decrypt(user.Password); //The password comes encrypted from the frontend
            password = BCrypt.Net.BCrypt.HashPassword(password); //Hashing the password for security

            //Assign initial role as "User".
            var role = _context.Roles.Where(a => a.RoleName == "User").FirstOrDefault();
            var roles = new List<Role>() { role };

            User newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = password,
                Roles = roles
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return await Task.FromResult(response);
        }
    }
}
