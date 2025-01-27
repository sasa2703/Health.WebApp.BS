using AutoMapper;
using FiscalCloud.WebApp.BS.API.Mapping;
using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Service;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;


namespace HealthManager.WebApp.BS.Presentation.Tests.Services
{
    public class TrialServiceTests
    {
        private readonly Mock<ILogger<TrialService>> _mockLogger;
        private Mock<IRepositoryManager> _repositoryMock;
        private Mock<IConfiguration> _mockConfiguration;

        public TrialServiceTests()
        {
            // Initialize the mock logger
            _mockLogger = new Mock<ILogger<TrialService>>();
            _repositoryMock = new Mock<IRepositoryManager>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["FileUploadSettings:MaxFileSizeInMB"]).Returns("5");
        }

        [Fact]
        public async void GetHealth_Returns_OK()
        {
            // Arrange          
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);

            _repositoryMock
                .Setup(r => r.Trial.GetTrialAsync("1", false))
                .Returns(Task.FromResult(new ClinicalTrialMetadata { Title = "Andol", TrialId = "1" }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object,_mockLogger.Object);
            // Act
            var trial = await service.GetTrialAsync("1", false);

            // Assert
    
            Assert.Equal("Andol", trial.Title);
        }

        [Fact]
        public async void CreateTrial_Returns_OK()
        {
            // Arrange
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newTrial = new ClinicalTrialForCreationDto() {TrialId ="2", Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3),Status = "Not Started" };
            var validJson = JsonSerializer.Serialize(newTrial);
            var file = CreateMockFormFile(validJson, "application/json");

            _repositoryMock
                .Setup(r => r.Trial.CreateTrial(new ClinicalTrialMetadata { Title = "NFS", TrialId = "2", StartDate = DateTime.UtcNow.AddDays(-3) }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object, _mockLogger.Object);
            // Act
            var trial = await service.CreateTrialAsync(file);

            // Assert
            Assert.True(trial.IsSuccess);
            Assert.Empty(trial.ErrorMessage);
            Assert.NotNull(trial.TrialDto);
            Assert.Equal("NFS", trial.TrialDto.Title);
        }

        [Fact]
        public async void CreateTrial_Status_Y_Returns_Error()
        {
            // Arrange       
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newTrial = new ClinicalTrialForCreationDto() { TrialId = "2", Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3), Status = "Y" };
            var validJson = JsonSerializer.Serialize(newTrial);
            var file = CreateMockFormFile(validJson, "application/json");

            _repositoryMock
                .Setup(r => r.Trial.CreateTrial(new ClinicalTrialMetadata { Title = "NFS", TrialId = "2", StartDate = DateTime.UtcNow.AddDays(-3) }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object, _mockLogger.Object);
            // Act
            var trial = await service.CreateTrialAsync(file);

            // Assert
            Assert.False(trial.IsSuccess);
            Assert.NotEmpty(trial.ErrorMessage);
            Assert.Equal(trial.ErrorMessage, "JSON validation failed: NotInEnumeration: #/status");
        }

        [Fact]
        public async void CreateTrial_StatusOngoing_Returns_OKEndDate_And_Duration()
        {
            // Arrange
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newTrial = new ClinicalTrialForCreationDto() { TrialId = "2", Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3), Status = "Ongoing" };
            var validJson = JsonSerializer.Serialize(newTrial);
            var file = CreateMockFormFile(validJson, "application/json");

            _repositoryMock
                .Setup(r => r.Trial.CreateTrial(new ClinicalTrialMetadata { Title = "NFS", TrialId = "2", StartDate = DateTime.UtcNow.AddDays(-3) }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object, _mockLogger.Object);
            // Act
            var trial = await service.CreateTrialAsync(file);

            // Assert
            Assert.True(trial.IsSuccess);
            Assert.Empty(trial.ErrorMessage);
            Assert.NotNull(trial.TrialDto);
            Assert.Equal(trial.TrialDto.EndDate, newTrial.StartDate.AddMonths(1));
            Assert.Equal(trial.TrialDto.Duration, (trial.TrialDto.EndDate.Value - newTrial.StartDate).Days);
            Assert.Equal("NFS", trial.TrialDto.Title);
        }

        [Fact]
        public async void CreateTrial_EndDateBefforeStartDate_Returns_Error()
        {
            // Arrange;
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newHealth = new ClinicalTrialForCreationDto() { TrialId = "2", Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3), EndDate =DateTime.UtcNow.AddDays(-4), Status = "Ongoing" };
            var validJson = JsonSerializer.Serialize(newHealth);
            var file = CreateMockFormFile(validJson, "application/json");

            _repositoryMock
                .Setup(r => r.Trial.CreateTrial(new ClinicalTrialMetadata { Title = "NFS", TrialId = "2", StartDate = DateTime.UtcNow.AddDays(-3) }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object, _mockLogger.Object);
            // Act
            var trial = await service.CreateTrialAsync(file);

            // Assert
            Assert.False(trial.IsSuccess);
            Assert.NotEmpty(trial.ErrorMessage);
            Assert.Equal(trial.ErrorMessage, "StartDate cannot be after EndDate.");
            
        }

        [Fact]
        public async void GetAllAvailableHealth_Returns_OK()
        {
            // Arrange
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);          

            _repositoryMock
                .Setup(r => r.Trial.GetAllAvailableTrials(false))
                .Returns(Task.FromResult(new List<ClinicalTrialMetadata> { new ClinicalTrialMetadata { Title = "Andol" } }));

            var service = new TrialService(_repositoryMock.Object, mapper, _mockConfiguration.Object, _mockLogger.Object);
            // Act
            var trial = await service.GetPubliclyAvailableTrials(false);

            // Assert
            Assert.NotEmpty(trial);
            Assert.Equal("Andol", trial[0].Title);
        }

        private static IFormFile CreateMockFormFile(string content, string contentType)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(contentBytes);

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.Length).Returns(contentBytes.Length);
            mockFile.Setup(f => f.ContentType).Returns(contentType);

            return mockFile.Object;
        }
    }
}
