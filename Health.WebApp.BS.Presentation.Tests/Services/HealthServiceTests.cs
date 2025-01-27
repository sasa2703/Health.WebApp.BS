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
    public class HealthServiceTests
    {
        private readonly Mock<ILogger<TrialService>> _mockLogger;

        public HealthServiceTests()
        {
            // Initialize the mock logger
            _mockLogger = new Mock<ILogger<TrialService>>();

        }

        [Fact]
        public async void GetHealth_Returns_OK()
        {
            // Arrange

            var repositoryMock = new Mock<IRepositoryManager>();
            var healthService = new Mock<ITrialService>();
            var accessRightResolver = new Mock<IAccessRightsResolver>();
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["FileUploadSettings:MaxFileSizeInMB"]).Returns("5");
            IMapper mapper = new Mapper(configuration);

            repositoryMock
                .Setup(r => r.Trial.GetTrialAsync("1", false))
                .Returns(Task.FromResult(new ClinicalTrialMetadata { Title = "Andol", TrialId = "1" }));

            var service = new TrialService(repositoryMock.Object, mapper, mockConfiguration.Object,_mockLogger.Object);
            // Act
            var health = await service.GetTrialAsync("1", false);

            // Assert
    
            Assert.Equal("Andol", health.Title);
        }

        [Fact]
        public async void CreateTrial_Returns_OK()
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryManager>();
            var healthService = new Mock<ITrialService>();
            var accessRightResolver = new Mock<IAccessRightsResolver>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["FileUploadSettings:MaxFileSizeInMB"]).Returns("5");
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newHealth = new ClinicalTrialForCreationDto() {TrialId ="2", Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3),Status = "Not Started" };
            var validJson = JsonSerializer.Serialize(newHealth);
            var file = CreateMockFormFile(validJson, "application/json");

            repositoryMock
                .Setup(r => r.Trial.CreateTrial(new ClinicalTrialMetadata { Title = "NFS", TrialId = "2", StartDate = DateTime.UtcNow.AddDays(-3) }));

            var service = new TrialService(repositoryMock.Object, mapper, mockConfiguration.Object, _mockLogger.Object);
            // Act
            var health = await service.CreateTrialAsync(file);

            // Assert
            Assert.True(health.IsSuccess);
            Assert.Empty(health.ErrorMessage);
            Assert.NotNull(health.TrialDto);
            Assert.Equal("NFS", health.TrialDto.Title);
        }

        [Fact]
        public async void GetAllAvailableHealth_Returns_OK()
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryManager>();
            var healthService = new Mock<ITrialService>();
            var accessRightResolver = new Mock<IAccessRightsResolver>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["FileUploadSettings:MaxFileSizeInMB"]).Returns("5");
            var profile = new ClinicalTrialProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            var newHealth = new ClinicalTrialForCreationDto() {  Title = "NFS", StartDate = DateTime.UtcNow.AddDays(-3) };

            repositoryMock
                .Setup(r => r.Trial.GetAllAvailableTrials(false))
                .Returns(Task.FromResult(new List<ClinicalTrialMetadata> { new ClinicalTrialMetadata { Title = "Andol" } }));

            var service = new TrialService(repositoryMock.Object, mapper, mockConfiguration.Object, _mockLogger.Object);
            // Act
            var health = await service.GetPubliclyAvailableTrials(false);

            // Assert
            Assert.NotEmpty(health);
            Assert.Equal("Andol", health[0].Title);
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
