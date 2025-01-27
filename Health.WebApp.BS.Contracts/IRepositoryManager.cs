namespace HealthManager.WebApp.BS.Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IUserStatusRepository UserStatus { get; }
        ITrialRepository Trial { get; }
        IApiAccessTokenRepository ApiAccessTokenRepository { get; }

        Task SaveAsync();
    }
}
