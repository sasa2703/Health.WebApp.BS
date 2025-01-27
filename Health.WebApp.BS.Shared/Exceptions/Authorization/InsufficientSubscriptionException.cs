namespace HealthManager.WebApp.BS.Shared.Exceptions.Authorization
{
    public sealed class InsufficientSubscriptionException : UnauthorizedException
    {
        public InsufficientSubscriptionException(string principalsSubscriptionID, string subscriptionID) : base($"User with subscription ID {principalsSubscriptionID} does not have permission to access rescources with subscriptionID {subscriptionID}")
        {
        }
    }
}
