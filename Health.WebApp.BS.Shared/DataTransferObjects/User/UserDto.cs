namespace  HealthManager.WebApp.BS.Shared.DataTransferObjects.User
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string AuthZeroUserID { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public UserCategoryDto Category { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? Phone { get; set; }
        public string? MobilePhone { get; set; }
        public string CountryCode { get; set; }
        public string? TimeZone { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? Status { get; set; }
        public string? ProjectCode { get; set; }
        public DateTime? LastNotificationRead { get; set; }
    }
}