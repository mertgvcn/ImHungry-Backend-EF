using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces;
using ImHungryLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ImHungryBackendER.Services.ControllerServices.CustomerServices
{
    public class LocationService : ILocationService
    {
        private readonly ImHungryContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LocationService(ImHungryContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<JsonResult> GetUserLocationList()
        {
            var userID = _userService.GetCurrentUserID();
            var userLocationList = _context.Users
                                      .Where(user => user.Id == userID)
                                      .Include(a => a.Locations).FirstOrDefault()!.Locations.AsQueryable()
                                      .ProjectTo<LocationViewModel>(_mapper.ConfigurationProvider)
                                      .ToList();

            return new JsonResult(userLocationList);
        }

        public async Task AddLocation(AddLocationRequest request)
        {
            var newLocation = new UserLocation()
            {
                Title = request.LocationTitle,
                Province = request.Province,
                District = request.District,
                Neighbourhood = request.Neighbourhood,
                Street = request.Street,
                BuildingNo = request.BuildingNo,
                BuildingAddition = request.BuildingAddition,
                ApartmentNo = request.ApartmentNo,
                Note = request.Note,
            };

            var userID = _userService.GetCurrentUserID();
            var user = _context.Users.Where(a => a.Id == userID).Include(a => a.Locations).FirstOrDefault();
            var userLocations = user.Locations;

            if (userLocations is null)
                userLocations = new List<UserLocation>() { newLocation };
            else
                userLocations.Add(newLocation);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationByLocationID(long locationID)
        {
            var location = _context.UserLocations.Where(a => a.Id == locationID).FirstOrDefault();

            _context.UserLocations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }
}
