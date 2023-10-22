using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.AuthParams;
using WebAPI_Giris.Models.Parameters.TokenParams;
using WebAPI_Giris.Models.Parameters.UserParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class AuthService : IAuthService
    {
        private readonly ICryptionService cryptionService;
        private readonly ITokenService tokenService;
        private readonly IDbService dbService;
        private readonly IUserService userService;

        public AuthService(ICryptionService cryptionService, 
            ITokenService tokenService, 
            IDbService dbService, 
            IUserService userService)
        {
            this.cryptionService = cryptionService;
            this.tokenService = tokenService;
            this.dbService = dbService;
            this.userService = userService;
        }

        public async Task<int> GetIdByUsername(string username)
        {
            int userID = -1;
            string query = "select \"userID\" from \"Users\" where \"userName\"=@userName;";

            await dbService.CheckConnectionAsync();

            NpgsqlDataReader reader;
            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userName", username);

            try
            {
                reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    userID = (int)reader["userID"];
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.StackTrace);
            }

            await reader.CloseAsync();
            return userID;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new();
            NpgsqlDataReader reader;

            bool isLogin = false;
            string plainPassword = cryptionService.Decrypt(request.encryptedPassword);
            string query = $"select \"password\" from \"Users\" where \"userName\"=@userName;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userName", request.username);

            try
            {
                reader = await cmd.ExecuteReaderAsync();

                //valid username
                if (await reader.ReadAsync())
                {
                    string hashedPassword = (string)reader["password"]; //password in db
                    isLogin = BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword); //Checks password is valid or not
                }
                //else username or password is wrong

            }
            catch (Exception e)
            {
                throw new SystemException(e.StackTrace);
            }

            await reader.CloseAsync();

            if (isLogin)
            {
                int userID = await GetIdByUsername(request.username);
                var generatedToken = await tokenService.GenerateToken(new GenerateTokenRequest { userID = userID.ToString() });

                response.authenticateResult = true;
                response.authToken = generatedToken.token;
                response.accessTokenExpireDate = generatedToken.tokenExpireDate;
            }

            return await Task.FromResult(response);
        }

        public async Task<UserRegisterResponse> RegisterUserAsync(Users user)
        {
            UserRegisterResponse response = new();
            response.isSuccess = true;

            //check if username already exists
            if (await userService.VerifyUsername(new VerifyUsernameRequest() { username = user.userName }))
            {
                response.isSuccess = false;
                response.errorMessage = "Invalid username";
                return response;
            }
            dbService.CloseConnection(); //avoid command already progress error

            string password = cryptionService.Decrypt(user.password); //The password comes encrypted from the frontend
            password = BCrypt.Net.BCrypt.HashPassword(password); //Hashing the password for security

            string query = $"insert into \"Users\" (\"firstName\",\"lastName\",\"userName\",\"email\",\"phoneNumber\",\"password\") " +
                                          $"values (@firstName, @lastName, @userName, @email, @phoneNumber, @password);";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@firstName", user.firstName);
            cmd.Parameters.AddWithValue("@lastName", user.lastName);
            cmd.Parameters.AddWithValue("@userName", user.userName);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                response.isSuccess = false;
                response.errorMessage = e.Message;
            }

            return response;
        }
    }
}
