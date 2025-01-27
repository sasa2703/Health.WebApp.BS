namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.ApiAccessToken
{
    public class ApiAccessTokenDto
    {    
        public int ApiAccessTokenId { get; set; }
        public string LoginId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string KeyVaultSecretId { get; set; } = null!;
    }
}
