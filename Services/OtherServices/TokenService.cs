using ImHungryBackendER;
using ImHungryBackendER.Models.ParameterModels;
using ImHungryLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class TokenService : ITokenService
    {
        private readonly ImHungryContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(ImHungryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]));
            
            var expireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(500));
            var claims = PrepareClaims(request.UserID, request.Roles);

            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: _configuration["AppSettings:ValidIssuer"],
                    audience: _configuration["AppSettings:ValidAudience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expireDate,
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new GenerateTokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = expireDate
            });
        }

        public List<Claim> PrepareClaims(string userID, List<Role> roles)
        {
            //Always need ID
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userID),
            };

            //If a restaurant owner, add the ID of the restaurant he owns.
            foreach (Role role in roles)
            {
                if(role.Id == 3)
                {
                    var restaurantID = _context.Restaurants.Where(x => x.OwnerId == long.Parse(userID))
                                          .Select(x => x.Id).FirstOrDefault().ToString();

                    claims.Add(new Claim("RestaurantID", restaurantID));
                }
            }

            //May have more than one claim
            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                claims.Add(new Claim("Role", role.RoleName)); //required for frontend fetch.
            });

            return claims;
        } 
    }
}
