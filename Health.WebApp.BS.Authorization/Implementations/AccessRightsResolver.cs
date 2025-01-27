using HealthManager.WebApp.BS.Authorization.IdentityTools;
using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;
using HealthManager.WebApp.BS.Shared.Exceptions.Auth0;
using HealthManager.WebApp.BS.Shared.Exceptions.Authorization;
using HealthManager.WebApp.BS.Shared.Exceptions.User;
using System.Security.Claims;


namespace HealthManager.WebApp.BS.Authorization.Implementations
{
    public class AccessRightsResolver : IAccessRightsResolver
    {
        private readonly IUserService _userService;

        public AccessRightsResolver(IUserService userService)
        {
            _userService = userService;
        }

        public async Task CheckPrincipalsRightsOnDelete(ClaimsPrincipal principal, string usernameToDelete)
        {
            var user = await _userService.GetUserByUsernameAsync(usernameToDelete, false);
            if(user == null)
            {
                throw new UserNotFoundException(usernameToDelete);
            }

            string principalUsername = ClaimsParser.ParseClaim(principal, TokenClaims.Username);
            if(principalUsername == user.Username)
            {
                throw new PrincipalSelfDeleteException(usernameToDelete);
            }

            CheckPrincipalsRightsOnSubscription(principal, usernameToDelete);

        }

        public void CheckPrincipalsRightsOnSubscription(ClaimsPrincipal principal, string subscriptionID)
        {
            string categoryID = ClaimsParser.ParseClaim(principal, TokenClaims.UserCategory);
            int categoryIDNumber = 0;
            if (!int.TryParse(categoryID, out categoryIDNumber))
            {
                throw new TokenInvalidException($"Claim categoryID is not a valid integer.");
            }

            if (categoryIDNumber == UserCategory.Internal)
            {
                return;
            }

            if (categoryIDNumber == UserCategory.Partner)
            {
                throw new NotImplementedException();
            }

            string principalsSubscriptionID = ClaimsParser.ParseClaim(principal, TokenClaims.SubscriptionId);
            if (principalsSubscriptionID != subscriptionID)
            {
                throw new InsufficientSubscriptionException(principalsSubscriptionID, subscriptionID);
            }
        }    

        public void CheckPrincipalsUsername(ClaimsPrincipal principal, string username)
        {
            string principalUsername = ClaimsParser.ParseClaim(principal, TokenClaims.Username);

            if (principalUsername != username)
            {
                throw new InvalidPrincipalUsernameException(principalUsername, username);
            }
        }
    }
}
