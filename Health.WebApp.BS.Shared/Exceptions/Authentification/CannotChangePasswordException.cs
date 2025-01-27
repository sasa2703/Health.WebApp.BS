namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class CannotChangePasswordException : InternalServerErrorException
    {
        public CannotChangePasswordException(string username) : base($"There was an error changing password for user {username}")
        {
        }
    }
}
