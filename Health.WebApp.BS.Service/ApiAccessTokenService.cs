using AutoMapper;
using HealthManager.WebApp.BS.Authorization.IdentityTools;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.ApiAccessToken;
using HealthManager.WebApp.BS.Shared.Exceptions.ApiAccessToken;
using HealthManager.WebApp.BS.Shared.Exceptions.Auth0;
using HealthManager.WebApp.BS.Shared.Exceptions.User;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HealthManager.WebApp.BS.Service
{
    public class ApiAccessTokenService :IApiAccessTokenService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiAccessTokenService> _logger;


        public ApiAccessTokenService(IRepositoryManager repository,IMapper mapper, ILogger<ApiAccessTokenService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;         
        }    

        public async Task<(IEnumerable<ApiAccessTokenDto> apiAccessTokenDto, MetaData metaData)> GetApiAccessTokensAsync(ApiAccessTokenParameters apiAccessTokenParam, bool trackChanges, ClaimsPrincipal user)
        {
            int categoryIDNumber = GetCategoryId(user);

            string subscriptionID = ClaimsParser.ParseClaim(user, TokenClaims.SubscriptionId);

            if (categoryIDNumber == Shared.Constants.UserCategory.Internal)
            {
                var apiAccessTokenWithMetaData = await _repository.ApiAccessTokenRepository.GetAllApiAccessTokenAsync(apiAccessTokenParam, trackChanges);
                return MapApiAccessToken(apiAccessTokenWithMetaData);
            }

            if (categoryIDNumber == Shared.Constants.UserCategory.Partner || categoryIDNumber == Shared.Constants.UserCategory.EndUser)
            {
                var apiAccessTokenWithMetaData = await _repository.ApiAccessTokenRepository.GetApiAccessTokenForExternalUserAsync(apiAccessTokenParam, trackChanges, subscriptionID);
                return MapApiAccessToken(apiAccessTokenWithMetaData);
            }

            throw new UnknownUserCategoryException($"Unknown categoryID: {categoryIDNumber}");

        }

        private static int GetCategoryId(ClaimsPrincipal user)
        {
            string categoryID = ClaimsParser.ParseClaim(user, TokenClaims.UserCategory);

            int categoryIDNumber = 0;
            if (!int.TryParse(categoryID, out categoryIDNumber))
            {
                throw new TokenInvalidException($"Claim categoryID is not a valid integer.");
            }

            return categoryIDNumber;
        }

        private (IEnumerable<ApiAccessTokenDto> apiAccessTokenDto, MetaData metaData) MapApiAccessToken(PagedList<ApiAccessToken> apiAccessTokenWithMetaData)
        {
            var apiAccessTokenDTO = _mapper.Map<IEnumerable<ApiAccessTokenDto>>(apiAccessTokenWithMetaData);
            return (apiAccessTokenDto: apiAccessTokenDTO, metaData: apiAccessTokenWithMetaData.MetaData);
        }
    

        public async Task<ApiAccessTokenDto> CreateApiAccessTokenAsync(ApiAccessTokenForCreationDto apiAccessToken)
        {

            var apiAccessTokenEntity = _mapper.Map<ApiAccessToken>(apiAccessToken);
            var secretName = apiAccessToken.LoginId;

            apiAccessTokenEntity.KeyVaultSecretId = secretName;
            _repository.ApiAccessTokenRepository.CreateApiAccessToken(apiAccessTokenEntity);
            await _repository.SaveAsync();                  

            var apiAccessTokenToReturn = _mapper.Map<ApiAccessTokenDto>(apiAccessTokenEntity);

            return apiAccessTokenToReturn;
        }

        public async Task<ApiAccessTokenDto> GetApiAccessTokenByIdAsync(int id, bool trackChanges)
        {
            var apiAccessToken = await _repository.ApiAccessTokenRepository.GetApiAccessTokenByIdAsync(id, trackChanges);

            if (apiAccessToken is null)
            {
                throw new ApiAccessTokenNotFoundException(id.ToString());
            }

            var apiAccessTokenDto = _mapper.Map<ApiAccessTokenDto>(apiAccessToken);
            return apiAccessTokenDto;
        }

        public async Task DeleteApiAccessTokenAsync(int id, bool trackChanges)
        {
            var apiAccessToken = await _repository.ApiAccessTokenRepository.GetApiAccessTokenByIdAsync(id, trackChanges);
            if (apiAccessToken is null)
            {
                throw new ApiAccessTokenNotFoundException(id.ToString());
            }         

            try
            {
                _repository.ApiAccessTokenRepository.DeleteApiAccessToken(apiAccessToken);
                await _repository.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete api access token with name {apiAccessToken.KeyVaultSecretId} from key vault. Error: ${e.Message}");
                throw new Exception("Unable to delete access token from database.");
            }
        }


        private static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public async Task<ApiAccessTokenDto> GetToken(int id, ClaimsPrincipal user)
        {
            string subscriptionID = ClaimsParser.ParseClaim(user, TokenClaims.SubscriptionId);

            var apiAccessToken = await _repository.ApiAccessTokenRepository.GetApiAccessTokenByIdAsync(id, false);
            if (apiAccessToken is null)
            {
                throw new ApiAccessTokenNotFoundException(id.ToString());
            }

            int categoryIDNumber = GetCategoryId(user);

            if (categoryIDNumber != Shared.Constants.UserCategory.Internal && !apiAccessToken.SubscriptionId.Equals(subscriptionID))
            {
                string userName = ClaimsParser.ParseClaim(user, TokenClaims.Username);
                _logger.LogError($"User {userName} with subscriptionId {subscriptionID} try to retrieve token with {apiAccessToken.SubscriptionId} subscriptionId");
                throw new UserAndTokenSubscriptionIdAreNotEqual(userName, subscriptionID, apiAccessToken.Id);
            }

            var apiAccessTokenDto = _mapper.Map<ApiAccessTokenDto>(apiAccessToken);
            return apiAccessTokenDto;

        }
      
    }
}
