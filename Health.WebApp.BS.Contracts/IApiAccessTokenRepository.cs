using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.RequestFeatures;

namespace HealthManager.WebApp.BS.Contracts
{
    public interface IApiAccessTokenRepository
    {
        void CreateApiAccessToken(ApiAccessToken apiAccessToken);
        void DeleteApiAccessToken(ApiAccessToken apiAccessToken);
        Task<PagedList<ApiAccessToken>> GetAllApiAccessTokenAsync(ApiAccessTokenParameters requestParameters, bool trackChanges);
        Task<ApiAccessToken> GetApiAccessTokenByIdAsync(int id, bool trackChanges);
        Task<PagedList<ApiAccessToken>> GetApiAccessTokenForExternalUserAsync(ApiAccessTokenParameters requestParameters, bool trackChanges, string subscriptionID);
    }
}
