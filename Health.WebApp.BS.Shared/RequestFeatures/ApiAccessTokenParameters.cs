namespace HealthManager.WebApp.BS.Shared.RequestFeatures
{
    public class ApiAccessTokenParameters:RequestParameters
    {
        public int ApiAccessTokenId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
