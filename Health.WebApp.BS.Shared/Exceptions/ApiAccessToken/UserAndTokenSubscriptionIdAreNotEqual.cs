namespace HealthManager.WebApp.BS.Shared.Exceptions.ApiAccessToken
{
    public class UserAndTokenSubscriptionIdAreNotEqual : UnauthorizedException
    {
        public UserAndTokenSubscriptionIdAreNotEqual(string userName,string subscriptionID, int apiAccessTokenId) :base($"User {userName} with subscriptionId {subscriptionID} has no rights to Api access token with {apiAccessTokenId} Id ")
        {

        }
    }
}
