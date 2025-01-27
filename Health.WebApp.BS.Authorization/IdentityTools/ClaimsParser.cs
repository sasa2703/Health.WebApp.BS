using HealthManager.WebApp.BS.Shared.Exceptions.Auth0;
using System.Security.Claims;

namespace HealthManager.WebApp.BS.Authorization.IdentityTools
{
    public static class ClaimsParser
    {
        public static string ParseClaim(ClaimsPrincipal user, string claim)
        {
            try
            {
                string parsedValue = user.Claims.First(x => x.Type == claim).Value;
                return parsedValue;
            }catch
            {
                throw new TokenInvalidException($"Claim {claim} is not present in the access token");
            }
        }

    }
}
