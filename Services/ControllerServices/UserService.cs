using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using ImHungryBackendER.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI_Giris.Models;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class UserService : IUserService
    {
        private readonly ImHungryContext _context;
        private readonly IMapper _mapper;
        private readonly IDbOperationHelperService _dbOperationHelperService;
        private readonly ICryptionService _cryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            ImHungryContext context, 
            IMapper mapper,
            IDbOperationHelperService dbOperationHelperService,
            ICryptionService cryptionService, 
            IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _mapper = mapper;
            _dbOperationHelperService = dbOperationHelperService;
            _cryptionService = cryptionService;
            _httpContextAccessor = httpContextAccessor;
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
        public long GetCurrentUserID()
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(result);
            }
            
            return -1;
        }


        //Get account info, not include password
        public async Task<JsonResult> GetAccountInfo()
        {
            var accountInfo = _context.Users.Where(user => user.Id == GetCurrentUserID())
                                 .ProjectTo<UserAccountViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();

            return new JsonResult(accountInfo);
        }

        public async Task SetAccountInfo(UserAccountViewModel request)
        {
            User user = new User()
            {
                Id = GetCurrentUserID(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
             };

            _context.Update(user);
            _dbOperationHelperService.MarkModifiedProperties<User>(user, _context);
            await _context.SaveChangesAsync();
        }

        //Get current location of user, may be null.
        public async Task<JsonResult> GetCurrentLocation()
        {
            var userID = GetCurrentUserID();
            var currentLocation = _context.Users.Where(user => user.Id == userID)
                                    .Include(a => a.CurrentLocation).Select(a => a.CurrentLocation)
                                    .ProjectTo<LocationViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();

            return new JsonResult(currentLocation);
        }

        public async Task SetCurrentLocation(long locationID)
        {
            var userID = GetCurrentUserID();
            var newCurrentLocation = _context.UserLocations.Where(a => a.Id == locationID).FirstOrDefault();

            User user = new User()
            {
                Id = userID,
                CurrentLocation = newCurrentLocation,
            };

            _context.Update(user);
            _dbOperationHelperService.MarkModifiedProperties<User>(user, _context);

            //in mark modified properties if property is null, it marks at nonmodified. So need to check it.
            if (newCurrentLocation == null)
                _context.Entry(user).Reference(a => a.CurrentLocation).IsModified = true;

            await _context.SaveChangesAsync();
        }

        //Check db if username and email exists
        public async Task<bool> VerifyUsername(string username)
        {
            var user = _context.Users.Where(a => a.Username == username).FirstOrDefault();

            return (user == null) ? false : true;
        }

        public async Task<bool> VerifyEmail(string email)
        {
            var user = _context.Users.Where(a => a.Email == email).FirstOrDefault();

            return (user == null) ? false : true;
        }

        public async Task<bool> VerifyPassword(string plainPassword)
        {
            var userID = GetCurrentUserID();
            var user = _context.Users.Where(user => user.Id == userID).FirstOrDefault();

            return (user==null) ? false : BCrypt.Net.BCrypt.Verify(plainPassword, user.Password); //user.Password is hashed password
        }

        //Password operations
        public async Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request)
        {
            //Mailine yeni şifre gönder
            return await VerifyEmail(request.Email);
        }

        public async Task ChangePassword(string encryptedPassword)
        {
            var userID = GetCurrentUserID();
            //string newPassword = _cryptionService.Decrypt(encryptedPassword); //Convert encrypted password that comes from frontend to plain password
            string newPassword = BCrypt.Net.BCrypt.HashPassword(encryptedPassword); //password hashing

            User user = new User()
            {
                Id = userID,
                Password = newPassword,
            };

            _context.Update(user);
            _dbOperationHelperService.MarkModifiedProperties<User>(user, _context);
            await _context.SaveChangesAsync();
        }
    }
}



/*
 * private readonly IServiceScopeFactory _scopeFactory;
 * 
 * Metodlarda kullanarak gereksiz kullanımı engelleriz.
    var scope = _scopeFactory.CreateScope();
    var userService = scope.ServiceProvider.GetService<IUserService>();
 */