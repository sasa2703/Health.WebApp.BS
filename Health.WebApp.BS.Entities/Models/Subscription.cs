using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class Subscription
    {
        public Subscription()
        {
            ApiAccessTokens = new HashSet<ApiAccessToken>();
            Users = new HashSet<User>();
        }

        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("subscriptionName")]
        public string SubscriptionName { get; set; } = null!;

        [JsonPropertyName("projectCode")]
        public string ProjectCode { get; set; } = null!;

        [JsonPropertyName("dtCreated")]
        public DateTime? DtCreated { get; set; }

        [JsonPropertyName("dtLastUpdate")]
        public DateTime? DtLastUpdate { get; set; }

        [JsonPropertyName("iUserCategory")]
        public virtual UserCategory? IUserCategory { get; set; }

        [JsonPropertyName("apiAccessTokens")]
        public virtual ICollection<ApiAccessToken> ApiAccessTokens { get; set; }

        [JsonPropertyName("users")]
        public virtual ICollection<User> Users { get; set; }
    }
}
