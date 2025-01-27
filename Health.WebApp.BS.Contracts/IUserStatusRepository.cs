using HealthManager.WebApp.BS.Entities.Models;


namespace HealthManager.WebApp.BS.Contracts
{
    public interface IUserStatusRepository
    {
        Task<UserStatus> GetUserStatusByName(string name);
    }
}
