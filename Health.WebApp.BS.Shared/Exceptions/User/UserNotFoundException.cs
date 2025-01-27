namespace HealthManager.WebApp.BS.Shared.Exceptions.User
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int userId) : base($"The user with id: {userId} doesn't exist in the database.")
        {

        }

        public UserNotFoundException(string username) : base($"The user with username: {username} doesn't exist in the database.")
        {

        }
    }
}
