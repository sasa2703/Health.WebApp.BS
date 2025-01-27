
namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class TokenInvalidException : UnauthorizedException
    {
        public TokenInvalidException(string? message) : base(message)
        {
        }
    }
}
