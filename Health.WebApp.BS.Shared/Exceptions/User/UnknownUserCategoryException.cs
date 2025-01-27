namespace HealthManager.WebApp.BS.Shared.Exceptions.User
{
    public class UnknownUserCategoryException : BadRequestException
    {
        public UnknownUserCategoryException(string? message) : base(message)
        {
        }
    }
}
