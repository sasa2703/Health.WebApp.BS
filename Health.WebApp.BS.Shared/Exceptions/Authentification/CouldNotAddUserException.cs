namespace HealthManager.WebApp.BS.Shared.Exceptions.Auth0
{
    public class CouldNotAddUserException : InternalServerErrorException
    {
        public CouldNotAddUserException(string message) : base($"Could not add user to Auth 0, Error: {message}")
        {
        }
    }
}
