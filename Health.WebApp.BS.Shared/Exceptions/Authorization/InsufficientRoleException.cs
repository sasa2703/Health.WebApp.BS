namespace HealthManager.WebApp.BS.Shared.Exceptions.Authorization
{
    public sealed class InsufficientRoleException : UnauthorizedException
    {
        public InsufficientRoleException(string requiredRole, string usersRole) : base($"User with role {usersRole} cannot access resources for role {requiredRole}")
        {
        }
    }
}
