using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Claims;
using WebAPI_Giris.Models;
using WebAPI_Giris.Models.Parameters.UserParams;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class UserService : IUserService
    {
        private readonly IDbService dbService;
        private readonly ICryptionService cryptionService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IDbService dbService, ICryptionService cryptionService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbService = dbService;
            this.cryptionService = cryptionService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Get general user informations
        public async Task<JsonResult> GetUserInfo()
        {
            var accountInfo = await GetAccountInfo();
            var currentLocation = await GetCurrentLocation();

            var userInfo = new
            {
                accountInfo = accountInfo.Value,
                currentLocation = currentLocation.Value
            };

            return new JsonResult(userInfo);
        }


        //Get user id from claim that inside the api token
        public int GetCurrentUserID()
        {
            if (httpContextAccessor.HttpContext is not null)
            {
                var result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(result);
            }

            return -1;
        }


        //Get account info, not include password
        public async Task<JsonResult> GetAccountInfo()
        {
            JsonResult jsonResult = new JsonResult(null);

            int userID = GetCurrentUserID();
            string query = "select \"firstName\",\"lastName\",\"userName\",\"email\",\"phoneNumber\" from \"Users\" where \"userID\"=@userID";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var accountInfo = new
                        {
                            firstName = (string)reader["firstName"],
                            lastName = (string)reader["lastName"],
                            userName = (string)reader["userName"],
                            email = (string)reader["email"],
                            phoneNumber = (string)reader["phoneNumber"],
                        };

                        jsonResult = new JsonResult(accountInfo);
                    }
                }
            }
            catch (Exception e)
            {
                if(httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            return jsonResult;
        }

        public async Task<bool> SetAccountInfo(SetAccountInfoRequest request)
        {
            int userID = GetCurrentUserID();
            string query = $"update \"Users\" " +
               $"set \"firstName\" = @firstName, \"lastName\"= @lastName, \"userName\"=@userName, \"email\"= @email, \"phoneNumber\"= @phoneNumber " +
               $"where \"userID\" = @userID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@firstName", request.firstName);
            cmd.Parameters.AddWithValue("@lastName", request.lastName);
            cmd.Parameters.AddWithValue("@userName", request.userName);
            cmd.Parameters.AddWithValue("@email", request.email);
            cmd.Parameters.AddWithValue("@phoneNumber", request.phoneNumber);
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }


        //Get current location of user, may be null.
        public async Task<JsonResult> GetCurrentLocation()
        {
            JsonResult jsonResult = new JsonResult(null);

            int userID = GetCurrentUserID();
            string query = "select \"locationTitle\",\"province\",\"district\",\"neighbourhood\",\"street\",\"buildingNo\",\"buildingAddition\",\"apartmentNo\",\"note\" " +
               "from \"Users\" " +
               "natural join \"User_location\" " +
               "where \"userID\"=@userID";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (await reader.ReadAsync())
                    {
                        var currentLocation = new
                        {
                            locationTitle = (string)reader["locationTitle"],
                            province = (string)reader["province"],
                            district = (string)reader["district"],
                            neighbourhood = (string)reader["neighbourhood"],
                            street = (string)reader["street"],
                            buildingNo = (string)reader["buildingNo"],
                            buildingAddition = (string)reader["buildingAddition"],
                            apartmentNo = (string)reader["apartmentNo"],
                            note = (string)reader["note"],
                        };

                        jsonResult = new JsonResult(currentLocation);
                    }
                }
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }

            return jsonResult;
        }

        public async Task<bool> SetCurrentLocation(SetCurrentLocationRequest request)
        {
            int userID = GetCurrentUserID();

            if (request.locationID < 0)
            {
                request.locationID = null;
            }

            string query = $"update \"Users\" " +
                           $"set \"locationID\" = @locationID " +
                           $"where \"userID\" = @userID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@locationID", request.locationID);
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }


        //Check db if username and email exists
        public async Task<bool> VerifyUsername(VerifyUsernameRequest request)
        {
            string query = "select \"userName\" from \"Users\" where \"userName\"=@username";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@username", request.username);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync()) //if username exists on db, return true
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }

        public async Task<bool> VerifyEmail(VerifyEmailRequest request)
        {
            string query = "select \"email\" from \"Users\" where \"email\"=@email";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@email", request.email);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }

        public async Task<bool> VerifyPassword(VerifyPasswordRequest request)
        {
            int userID = GetCurrentUserID();
            string query = "select \"password\" from \"Users\" where \"userID\"=@userID";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    //valid userID
                    if (await reader.ReadAsync())
                    {
                        string hashedPassword = (string)reader["password"]; //password in db

                        bool isMatch = BCrypt.Net.BCrypt.Verify(request.password, hashedPassword); //Checks password is valid or not
                        return isMatch;
                    }

                    //wrong userID
                    return false;
                }
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }


        //Password operations
        public async Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request)
        {
            bool isValidEmail = await VerifyEmail(new VerifyEmailRequest() { email = request.email });
            dbService.CloseConnection(); //avoid command already in progress error.

            if (isValidEmail)
            {
                string query = "update \"Users\" " +
                               "set \"password\"=@password " +
                               "where \"email\"=@email;";

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.plainPassword);

                await dbService.CheckConnectionAsync(); //reopen connection

                NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@email", request.email);

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception e)
                {
                    if (httpContextAccessor.HttpContext is not null) 
                        this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                    throw new SystemException(e.StackTrace);
                }
            }
            else
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                return false;
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            int userID = GetCurrentUserID();
            string newPassword = cryptionService.Decrypt(request.encryptedPassword);

            //password hashing
            newPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            string query = $"update \"Users\" " +
                           $"set \"password\"=@newPassword " +
                           $"where \"userID\"=@userID;";

            await dbService.CheckConnectionAsync();

            NpgsqlCommand cmd = new NpgsqlCommand(query, dbService.GetConnection());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@newPassword", newPassword);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                if (httpContextAccessor.HttpContext is not null) 
                    this.httpContextAccessor.HttpContext.Response.StatusCode = 400;
                throw new SystemException(e.StackTrace);
            }
        }
    }
}
