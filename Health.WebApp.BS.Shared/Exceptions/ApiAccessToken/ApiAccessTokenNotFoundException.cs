namespace HealthManager.WebApp.BS.Shared.Exceptions.ApiAccessToken
{
    public class ApiAccessTokenNotFoundException : NotFoundException
    {
        public ApiAccessTokenNotFoundException(string? id) : base($"The api access token with id: {id} doesn't exist in the database.")
        {
        }
    }
}
