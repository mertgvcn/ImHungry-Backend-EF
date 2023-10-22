using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Giris.Models.Parameters.TokenParams;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"]));
            var expireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(500));

            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: configuration["AppSettings:ValidIssuer"],
                    audience: configuration["AppSettings:ValidAudience"],
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, request.userID)
                    },
                    notBefore: DateTime.UtcNow,
                    expires: expireDate,
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new GenerateTokenResponse
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwt),
                tokenExpireDate = expireDate
            });
        }
    }
}
