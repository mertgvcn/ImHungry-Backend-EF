using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using ImHungryBackendER.Services.OtherServices.Interfaces;
using ImHungryLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace ImHungryBackendER.Services.ControllerServices
{
    #pragma warning disable //removes error lines
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
                var userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(userID);
            }
            
            return -1;
        }

        //Get user roles from claim that inside the api token
        public List<string> GetCurrentUserRoles()
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                var roleClaims = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role);
                var roles = roleClaims.Select(a => a.Value).ToList();
                return roles;
            }

            return null;
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
                                     .FirstOrDefault();

            if(currentLocation != null)
            {
                return new JsonResult(_mapper.Map<LocationViewModel>(currentLocation));
            }

            return new JsonResult(null);
        }

        public async Task SetCurrentLocation(SetCurrentLocationRequest request)
        {
            var userID = GetCurrentUserID();
            var newCurrentLocation = _context.UserLocations.Where(a => a.Id == request.LocationID).FirstOrDefault();

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
        public async Task<bool> VerifyUsername(VerifyUsernameRequest request)
        {
            var user = _context.Users.Where(a => a.Username == request.Username).FirstOrDefault();

            return (user == null) ? false : true;
        }

        public async Task<bool> VerifyEmail(VerifyEmailRequest request)
        {
            var user = _context.Users.Where(a => a.Email == request.Email).FirstOrDefault();

            return (user == null) ? false : true;
        }

        public async Task<bool> VerifyPassword(VerifyPasswordRequest request)
        {
            var userID = GetCurrentUserID();
            var user = _context.Users.Where(user => user.Id == userID).FirstOrDefault();

            return (user==null) ? false : BCrypt.Net.BCrypt.Verify(request.PlainPassword, user.Password); //user.Password is hashed password
        }

        //Password operations
        public async Task<bool> iForgotMyPassword(iForgotMyPasswordRequest request)
        {
            //Mailine yeni şifre gönder
            return await VerifyEmail(new VerifyEmailRequest { Email = request.Email });
        }

        public async Task ChangePassword(ChangePasswordRequest request)
        {
            var userID = GetCurrentUserID();
            string plainPassword = _cryptionService.Decrypt(request.EncryptedPassword); //Convert encrypted password that comes from frontend to plain password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword); //password hashing

            User user = new User()
            {
                Id = userID,
                Password = hashedPassword,
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