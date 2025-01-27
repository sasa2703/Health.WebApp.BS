using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class UserCategory
    {
        public UserCategory()
        {
            Subscriptions = new HashSet<Subscription>();
            Users = new HashSet<User>();
        }
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userCategoryName")]
        public string UserCategoryName { get; set; } = null!;

        [JsonPropertyName("subscriptions")]
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        [JsonPropertyName("users")]
        public virtual ICollection<User> Users { get; set; }
    }
}
