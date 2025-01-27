using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;

namespace HealthManager.WebApp.BS.Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<ITrialRepository> _healthRepository;
        private readonly Lazy<IUserStatusRepository> _userStatusRepository;           
        private readonly Lazy<IApiAccessTokenRepository> _apiAccessTokenRepository;      

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            _healthRepository = new Lazy<ITrialRepository>(() => new TrialRepository(repositoryContext));
            _userStatusRepository = new Lazy<IUserStatusRepository>(() => new UserStatusRepository(repositoryContext));
            _apiAccessTokenRepository = new Lazy<IApiAccessTokenRepository>(() => new ApiAccessTokenRepository(repositoryContext));
        }

        public IUserRepository User => _userRepository.Value;
       
        public ITrialRepository Trial => _healthRepository.Value;
        public IUserStatusRepository UserStatus => _userStatusRepository.Value;
                 
        public IApiAccessTokenRepository ApiAccessTokenRepository => _apiAccessTokenRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();


    }
}
