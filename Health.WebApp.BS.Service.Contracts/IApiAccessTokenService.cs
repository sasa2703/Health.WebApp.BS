using HealthManager.WebApp.BS.Shared.DataTransferObjects.ApiAccessToken;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using System.Security.Claims;

namespace HealthManager.WebApp.BS.Service.Contracts
{
    public interface IApiAccessTokenService
    {
        Task<ApiAccessTokenDto> CreateApiAccessTokenAsync(ApiAccessTokenForCreationDto apiAccessToken);
        Task DeleteApiAccessTokenAsync(int id, bool trackChanges);
        Task<ApiAccessTokenDto> GetApiAccessTokenByIdAsync(int id, bool trackChanges);
        Task<(IEnumerable<ApiAccessTokenDto> apiAccessTokenDto, MetaData metaData)> GetApiAccessTokensAsync(ApiAccessTokenParameters apiAccessTokenParam, bool trackChanges, ClaimsPrincipal user);
        Task<ApiAccessTokenDto> GetToken(int id, ClaimsPrincipal user);
    }
}
