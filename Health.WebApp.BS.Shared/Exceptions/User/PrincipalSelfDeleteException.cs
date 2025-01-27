namespace HealthManager.WebApp.BS.Shared.Exceptions.User
{
    public sealed class PrincipalSelfDeleteException : BadRequestException
    {
        public PrincipalSelfDeleteException(string principalUsername) : base($"Principal with username {principalUsername} cannot delete himself.")
        {
        }
    }
}
