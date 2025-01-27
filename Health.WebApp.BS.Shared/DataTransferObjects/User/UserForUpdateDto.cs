using System.ComponentModel.DataAnnotations;

namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.User
{
    public class UserForUpdateDto
    {

        [MaxLength(length: 50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
        public string? Name { get; set; }

        [MaxLength(length: 50, ErrorMessage = "Maximum length for the Title is 50 characters.")]
        public string? Title { get; set; }

        [MaxLength(length: 50, ErrorMessage = "Maximum length for the Company is 50 characters.")]
        public string? Company { get; set; }

        [MaxLength(length: 50, ErrorMessage = "Maximum length for the Phone is 50 characters.")]
        public string? Phone { get; set; }

        [MaxLength(length: 50, ErrorMessage = "Maximum length for the Mobile phone is 50 characters.")]
        public string? MobilePhone { get; set; }

        [MaxLength(length: 3, ErrorMessage = "Maximum length for the Country code is 3 characters.")]
        public string? CountryCode { get; set; }

        [MaxLength(length: 6, ErrorMessage = "Maximum length for the Time zone is 6 characters.")]
        public string? TimeZone { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RoleID { get; set; }

        public bool Deleted { get; set; }

        public DateTime? LastNotificationRead { get; set; }
    }
}
