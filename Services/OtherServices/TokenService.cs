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
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
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
                new Claim(ClaimTypes.NameIdentifier, userID)
            };

            //May have more than one claim
            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            });

            return claims;
        } 
    }
}
