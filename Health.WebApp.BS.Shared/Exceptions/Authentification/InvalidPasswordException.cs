
namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class InvalidPasswordException : BadRequestException
    {
        public InvalidPasswordException(string username) : base($"Invalid password for user {username}")
        {
        }
    }
}
