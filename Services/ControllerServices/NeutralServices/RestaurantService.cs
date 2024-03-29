﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ImHungryBackendER.Services.ControllerServices.NeutralServices
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ImHungryContext _context;
        private readonly IMapper _mapper;

        public RestaurantService(ImHungryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JsonResult> GetRestaurantInformationByID(long restaurantID)
        {
            var restaurantDetails = await GetRestaurantSummaryByID(restaurantID);
            var menu = await GetRestaurantMenuByID(restaurantID);
            var categories = await GetCategoriesByID(restaurantID);

            var restaurantInfo = new
            {
                restaurantDetails = restaurantDetails.Value,
                menu = menu.Value,
                categories = categories.Value
            };

            return new JsonResult(restaurantInfo);
        }

        public async Task<JsonResult> GetRestaurantListByLocation(GetRestaurantListByLocationRequest request)
        {
            var restaurantList = _context.Restaurants
                                    .Where(a => a.Location.Province.ToLower() == request.Province.ToLower())
                                    .Where(a => a.Location.District.ToLower() == request.District.ToLower())
                                    .Include(a => a.Location)
                                    .ProjectTo<RestaurantListViewModel>(_mapper.ConfigurationProvider)
                                    .ToList();

            return new JsonResult(restaurantList);
        }

        public async Task<JsonResult> GetRestaurantSummaryByID(long restaurantID)
        {
            var restaurantBriefInformation = _context.Restaurants
                                                .Where(restaurant => restaurant.Id == restaurantID)
                                                .ProjectTo<RestaurantSummaryViewModel>(_mapper.ConfigurationProvider)
                                                .FirstOrDefault();

            return new JsonResult(restaurantBriefInformation);
        }

        public async Task<JsonResult> GetRestaurantMenuByID(long restaurantID)
        {
            var restaurantMenu = _context.Items
                                    .Where(a => a.Restaurant.Id == restaurantID)
                                    .Include(a => a.Category)
                                    .ProjectTo<ItemViewModel>(_mapper.ConfigurationProvider)
                                    .ToList().OrderBy(a => a.Category.Id);

            return new JsonResult(restaurantMenu);
        }

        public async Task<JsonResult> GetCategoriesByID(long restaurantID)
        {
            var categories = _context.Categories
                                .Where(a => a.RestaurantId == restaurantID)
                                .ToList();

            return new JsonResult(categories);
        }
    }
}
