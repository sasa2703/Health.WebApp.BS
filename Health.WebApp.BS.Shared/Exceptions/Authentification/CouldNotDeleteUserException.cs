
namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class CouldNotDeleteUserException : InternalServerErrorException
    {
        public CouldNotDeleteUserException(string userID, string authZeroMessage) :
            base($"Could not delete user with ID {userID}. Error: {authZeroMessage}")
        {
        }
    }
}
