namespace HealthManager.WebApp.BS.Shared.Exceptions.Authorization
{
    public sealed class InvalidPrincipalUsernameException : UnauthorizedException
    {
        public InvalidPrincipalUsernameException(string principalUsername, string passedUsername) : base($"Principal's username {principalUsername} and passed username {passedUsername} don't match.")
        {
        }
    }
}
