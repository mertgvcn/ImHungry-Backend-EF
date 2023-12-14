using ImHungryBackendER.Models.ParameterModels;

namespace WebAPI_Giris.Services.OtherServices.Interfaces
{
    public interface ITokenService
    {
        Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);
    }
}
