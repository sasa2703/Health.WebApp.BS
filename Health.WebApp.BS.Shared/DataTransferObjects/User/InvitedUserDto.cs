using System.ComponentModel.DataAnnotations;

namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.User
{
    public class InvitedUserDto
    {
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(2)]
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        [Required]
        public string? CountryCode { get; set; }
        public string? TimeZone { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? Status { get; set; }
    }
}
