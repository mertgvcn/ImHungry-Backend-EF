using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryBackendER.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAPI_Giris.Models;
using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace WebAPI_Giris.Services.ControllerServices
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ImHungryContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CreditCardService(ImHungryContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<JsonResult> GetUserCreditCards()
        {
            var userID = _userService.GetCurrentUserID();
            var userCreditCards = _context.Users
                                     .Where(user => user.Id == userID)
                                     .Include(a => a.CreditCards).FirstOrDefault()!.CreditCards.AsQueryable()
                                     .ProjectTo<CreditCardViewModel>(_mapper.ConfigurationProvider)
                                     .ToList();

            return new JsonResult(userCreditCards);
        }

        public async Task AddCreditCard(AddCreditCardRequest request)
        {
            var newCreditCard = new CreditCard()
            {
                Number = request.creditCardNumber,
                HolderName = request.creditCardHolderName,
                ExpirationDate = request.expirationDate,
                CVV = request.cvv
            };
            
            var userID = _userService.GetCurrentUserID();
            var user = _context.Users.Where(a => a.Id == userID).Include(a => a.CreditCards).FirstOrDefault();
            var userCreditCards = user.CreditCards;

            if (userCreditCards is null)          
                userCreditCards = new List<CreditCard>() { newCreditCard };
            else 
                userCreditCards.Add(newCreditCard);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCreditCardByID(long creditCardID)
        {
            var creditCard = _context.CreditCards.Where(a => a.Id == creditCardID).FirstOrDefault();

            _context.CreditCards.Remove(creditCard);
            await _context.SaveChangesAsync();
        }
    }
}
