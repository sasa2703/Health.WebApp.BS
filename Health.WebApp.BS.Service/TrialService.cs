using AutoMapper;
using Health.WebApp.BS.Shared.Schema;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Product;
using HealthManager.WebApp.BS.Shared.Exceptions.Health;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NJsonSchema.Validation;
using System.Text.Json;

namespace HealthManager.WebApp.BS.Service
{
    public class TrialService : ITrialService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TrialService> _logger;

        public TrialService(IRepositoryManager repository, IMapper mapper, IConfiguration configuration, ILogger<TrialService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(IEnumerable<ClinicalTrialDto> trial, MetaData metaData)> GetAllTrialsAsync(TrialParameters trialParameters, bool trackChanges)
        {
            var trialWithMetaData = await _repository.Trial.GetAllTrialsAsync(trialParameters, trackChanges);

            var trialDto = _mapper.Map<IEnumerable<ClinicalTrialDto>>(trialWithMetaData);

            return (trialDtos: trialDto, metaData: trialWithMetaData.MetaData);
        }

        public async Task<ClinicalTrialDto> GetTrialAsync(string trialId, bool trackChanges)
        {
            var product = await _repository.Trial.GetTrialAsync(trialId, trackChanges);

            if (product is null)
            {
                throw new TrialNotFoundException(trialId);
            }

            var productDto = _mapper.Map<ClinicalTrialDto>(product);

            return productDto;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, ClinicalTrialDto? TrialDto)> CreateTrialAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "No file was uploaded.", null);

            if (!file.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                return (false, "Only JSON files are allowed.", null);


            var maxFileSizeInMB = int.Parse(_configuration["FileUploadSettings:MaxFileSizeInMB"]);
            if (CheckMaxSizeOfFile(file, maxFileSizeInMB))
                return (false, $"File size exceeds the limit of {maxFileSizeInMB} MB.", null);

            try
            {
                string jsonContent = await ReadStream(file);

                var validationErrors = await GetValidator(jsonContent);

                if (validationErrors.Any())
                {
                    var errorMessages = string.Join("; ", validationErrors.Select(e => e.ToString()));
                    return (false, $"JSON validation failed: {errorMessages}", null);
                }

                var metadata = JsonSerializer.Deserialize<ClinicalTrialMetadata>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (metadata == null)
                    return (false, "Invalid JSON content.", null);

                if (metadata.StartDate > metadata.EndDate)
                {
                    return (false,"StartDate cannot be after EndDate.", null);
                }

                var healthEntity = _mapper.Map<ClinicalTrialMetadata>(metadata);
                healthEntity.TransformData();

                _repository.Trial.CreateTrial(healthEntity);
                await _repository.SaveAsync();

                var health = _mapper.Map<ClinicalTrialDto>(healthEntity);

                return (true, string.Empty, health);

            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error parsing JSON. Error: ${ex.Message}.");
                return (false, $"Error parsing JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error. Error: ${ex.Message}.");
                return (false, $"Internal server error: {ex.Message}", null);
            }

        }

        private async Task<ICollection<ValidationError>> GetValidator(string jsonContent)
        {
            // Validate JSON against schema
            var jsonSchema = JsonSchemaHelper.GetJsonSchema();

            var schema = await NJsonSchema.JsonSchema.FromJsonAsync(jsonSchema);
            var validator = new JsonSchemaValidator();

            var validationErrors = validator.Validate(jsonContent, schema);
            return validationErrors;
        }

        private async Task<string> ReadStream(IFormFile file)
        {
            // Read the file content
            var stream = file.OpenReadStream();
            var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();
            return jsonContent;
        }

        private bool CheckMaxSizeOfFile(IFormFile file, int maxFileSizeInMB)
        {
            // Get file size limit from configuration (optional)           
            var maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;

            if (file.Length > maxFileSizeInBytes)
                return true;

            return false;
        }

        public async Task DeleteTrialAsync(string trialId)
        {
            await _repository.Trial.DeleteTrialAsync(trialId);
        }


        public async Task<List<ClinicalTrialDto>> GetPubliclyAvailableTrials(bool trackChanges)
        {
            return _mapper.Map<List<ClinicalTrialDto>>(await _repository.Trial.GetAllAvailableTrials(trackChanges));
        }

        public async Task<(bool IsSuccess, string ErrorMessage, ClinicalTrialDto? TrialDto)> EditTrialAsync(string TrialId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "No file was uploaded.", null);

            if (!file.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                return (false, "Only JSON files are allowed.", null);


            var maxFileSizeInMB = int.Parse(_configuration["FileUploadSettings:MaxFileSizeInMB"]);
            if (CheckMaxSizeOfFile(file, maxFileSizeInMB))
                return (false, $"File size exceeds the limit of {maxFileSizeInMB} MB.", null);
            try
            {
                string jsonContent = await ReadStream(file);


                var validationErrors = await GetValidator(jsonContent);

                if (validationErrors.Any())
                {
                    var errorMessages = string.Join("; ", validationErrors.Select(e => e.ToString()));
                    return (false, $"JSON validation failed: {errorMessages}", null);
                }

                var editClinicalTrial = JsonSerializer.Deserialize<EditClinicalTrialDto>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (editClinicalTrial == null)
                    return (false, "Invalid JSON content.", null);


                ClinicalTrialMetadata existingTrial = await _repository.Trial.GetTrialAsync(TrialId, true);

                if (existingTrial == null)
                {
                    throw new TrialNotFoundException(TrialId);
                }

                _mapper.Map(editClinicalTrial, existingTrial);
                await _repository.SaveAsync();

                var clinicalTrialDto = _mapper.Map<ClinicalTrialDto>(existingTrial);
                return (true, string.Empty, clinicalTrialDto);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error parsing JSON. Error: ${ex.Message}.");
                return (false, $"Error parsing JSON: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error. Error: ${ex.Message}.");
                return (false, $"Internal server error: {ex.Message}", null);
            }
        }

        public async Task<ClinicalTrialDto> GetTrialByTrialIndexAsync(string trialIndex, bool trackChanges)
        {
            var trial = await _repository.Trial.GetTrialAsync(trialIndex, trackChanges);

            if (trial is null)
            {
                throw new TrialNotFoundException(trialIndex);
            }

            var trialDto = _mapper.Map<ClinicalTrialDto>(trial);

            return trialDto;
        }

    }
}
