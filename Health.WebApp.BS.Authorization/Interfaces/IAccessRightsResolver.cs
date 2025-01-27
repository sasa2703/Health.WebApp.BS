using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Authorization.Interfaces
{
    public interface IAccessRightsResolver
    {

        /// <summary>
        ///  Checks if user can access resources tied to a subscription passed by parameter subscriptionID (CRUD operations).
        ///  If subscriptionID is null or not passed, it is assumed user wants to access all subscriptions.
        /// </summary>
        /// <param name="principal">Claims principal with a token</param>
        /// <param name="subscriptionID">subscriptionID of resources</param>
        /// <exception>
        ///  Throws UnauthorizedException in case user does not have access rights
        ///  Throws InvalidTokenException in case of invalid token
        /// </exception>
        void CheckPrincipalsRightsOnSubscription(ClaimsPrincipal principal, string? subscriptionID = null);
        /// <summary>
        /// Checks if user has rights to access resources tied to a role passed with specific roleId.
        /// </summary>
        /// <param name="principal">Claims principal with a token</param>
        /// <param name="roleId">roleId of user</param>
      
        void CheckPrincipalsUsername(ClaimsPrincipal principal, string username);

        /// <summary>
        /// Checks if principal can delete user with passed username.
        /// </summary>
        /// <param name="principal">Claims principal with a token</param>
        /// <param name="usernameToDelete">username to delete</param>
        Task CheckPrincipalsRightsOnDelete(ClaimsPrincipal principal, string usernameToDelete);
    }
}
