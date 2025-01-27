
namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class CouldNotAcquireTokenException : BadRequestException
    {
        public CouldNotAcquireTokenException(string clientID, string domain) : base($"Could not acquire access token for client ID {clientID} and domain {domain}")
        {
        }
    }
}
