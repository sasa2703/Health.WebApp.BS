using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.RequestFeatures;

namespace HealthManager.WebApp.BS.Contracts
{
    public interface ITrialRepository
    {
        Task DeleteTrialAsync(string TrialId);
        Task<List<ClinicalTrialMetadata>> GetAllAvailableTrials(bool trackChanges);
        Task<PagedList<ClinicalTrialMetadata>> GetAllTrialsAsync(TrialParameters trialParameters,bool trackChanges);
        Task<ClinicalTrialMetadata> GetTrialByTrialIndexAsync(string TrialId, bool trackChanges);
        Task<ClinicalTrialMetadata> GetTrialAsync(string TrialId, bool trackChanges);
        void CreateTrial(ClinicalTrialMetadata healthEntity);
    }
}
