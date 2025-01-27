

namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class PasswordsDontMatchException : BadRequestException
    {
        public PasswordsDontMatchException() : base("Passwords don't match.")
        {
        }
    }
}
