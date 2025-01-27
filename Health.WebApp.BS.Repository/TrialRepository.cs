using Microsoft.EntityFrameworkCore;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.WebApp.BS.Repository
{
    public sealed class TrialRepository : RepositoryBase<ClinicalTrialMetadata>, ITrialRepository
    {
        public TrialRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task<PagedList<ClinicalTrialMetadata>> GetAllTrialsAsync(TrialParameters trialParameters, bool trackChanges)
        {
            IQueryable<ClinicalTrialMetadata> trials = FindAll(trackChanges);
           

            if (trialParameters.OnlyAvailable.HasValue && trialParameters.OnlyAvailable.Value)
            {
                trials = trials.Where(p => p.StartDate < DateTime.Now);
            }         

            return await PagedList<ClinicalTrialMetadata>
                .ToPageListAsync(trials, trialParameters.PageNumber, trialParameters.PageSize);
        }

        public async Task<ClinicalTrialMetadata> GetTrialByTrialIndexAsync(string trialId, bool trackChanges)
        {
            var trial = await FindByCondition(p => p.TrialId == trialId, trackChanges).SingleOrDefaultAsync();

            return trial;
        }

        public async Task<ClinicalTrialMetadata> GetTrialAsync(string trialIndex, bool trackChanges)
        {
            var trial = await FindByCondition(p => p.TrialId == trialIndex, trackChanges).SingleOrDefaultAsync();

            return trial;
        }

        public async Task DeleteTrialAsync(string trialId)
        {
            var trial = await FindByCondition(p => p.TrialId == trialId, true).SingleOrDefaultAsync();
            if (trial != null)
            {
                Delete(trial);
            }
        }

        public void CreateTrial(ClinicalTrialMetadata trial) 
        {
            ValidateModel(trial);

            Create(trial); 
        }

        private void ValidateModel<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true))
            {
                var errors = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
                throw new ValidationException($"Model validation failed: {errors}");
            }
        }

        public void UpdateTrial(ClinicalTrialMetadata trial) => Update(trial);


        public async Task<List<ClinicalTrialMetadata>> GetAllAvailableTrials(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderByDescending(x => x.StartDate).ToListAsync();
        }

    }
}
