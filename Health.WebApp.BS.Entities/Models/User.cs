using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class User
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("subscriptionId")]
        public string? SubscriptionId { get; set; }

        [JsonPropertyName("userCategory")]
        public int UserCategory { get; set; }

        [JsonPropertyName("roleId")]
        public int? RoleId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("timeZone")]
        public string? TimeZone { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }

        [JsonPropertyName("dtCreated")]
        public DateTime? DtCreated { get; set; }

        [JsonPropertyName("dtLastUpdate")]
        public DateTime? DtLastUpdate { get; set; }

        [JsonPropertyName("status")]
        public virtual UserStatus Status { get; set; } = null!;

        [JsonPropertyName("userCategoryNavigation")]
        public virtual UserCategory UserCategoryNavigation { get; set; } = null!;

        [JsonPropertyName("subscription")]
        public virtual Subscription? Subscription { get; set; }
    }
}
