using HealthManager.WebApp.BS.Shared.DataTransferObjects.Product;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace HealthManager.WebApp.BS.Service.Contracts
{
    public interface ITrialService
    {
        Task DeleteTrialAsync(string productId);
        Task<List<ClinicalTrialDto>> GetPubliclyAvailableTrials(bool trackChanges);
        Task<(IEnumerable<ClinicalTrialDto> trial, MetaData metaData)> GetAllTrialsAsync(TrialParameters trialParameters, bool trackChanges);
        Task<ClinicalTrialDto> GetTrialAsync(string trialId, bool trackChanges);
        Task<ClinicalTrialDto> GetTrialByTrialIndexAsync(string trialIndex, bool trackChanges);
        Task<(bool IsSuccess, string ErrorMessage, ClinicalTrialDto? TrialDto)> EditTrialAsync(string trialId, IFormFile file);
        Task<(bool IsSuccess, string ErrorMessage, ClinicalTrialDto? TrialDto)> CreateTrialAsync(IFormFile file);
    }
}
