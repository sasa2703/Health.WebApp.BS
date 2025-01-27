using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Users = new HashSet<User>();
        }
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("statusName")]
        public string StatusName { get; set; } = null!;

        [JsonPropertyName("users")]
        public virtual ICollection<User> Users { get; set; }
    }
}
