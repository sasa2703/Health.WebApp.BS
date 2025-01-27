namespace   HealthManager.WebApp.BS.Shared.Exceptions.User
{
    public sealed class UserAlreadyExistsException : BadRequestException
    {
        public UserAlreadyExistsException(int userId) : base($"The user with id: {userId} already exists in the database.")
        {

        }

        public UserAlreadyExistsException(string username) : base($"The user with username: {username} already exists in the database.")
        {

        }
    }
}
