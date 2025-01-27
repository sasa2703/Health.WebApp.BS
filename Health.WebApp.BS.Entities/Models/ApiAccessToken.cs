using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class ApiAccessToken
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("subscriptionId")]
        public string SubscriptionId { get; set; } = null!;

        [JsonPropertyName("subscriptionName")]
        public string SubscriptionName { get; set; } = null!;

        [JsonPropertyName("loginId")]
        public string LoginId { get; set; } = null!;

        [JsonPropertyName("dtCreated")]
        public DateTime? DtCreated { get; set; }

        [JsonPropertyName("dtExpireDate")]
        public DateTime? DtExpireDate { get; set; }

        [JsonPropertyName("keyVaultSecretId")]
        public string KeyVaultSecretId { get; set; } = null!;

        [JsonPropertyName("subscription")]

        public virtual Subscription Subscription { get; set; } = null!;
    }
}
