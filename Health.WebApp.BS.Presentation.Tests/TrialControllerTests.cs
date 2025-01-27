using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Presentation.Controllers;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HealthManager.WebApp.BS.Presentation.Tests
{
    public class TrialControllerTests
    {
        [Fact]
        public async void GetTrial_Returns_OK()
        {
            // Arrange
            var repositoryMock = new Mock<ITrialRepository>();
            var trialService = new Mock<ITrialService>();
            var accessRightResolver = new Mock<IAccessRightsResolver>();

            trialService
                .Setup(r => r.GetTrialAsync("1",false))             
                .Returns(Task.FromResult( new ClinicalTrialDto{ Title = "Test" }));

            var controller = new TrialController(trialService.Object, accessRightResolver.Object);

            // Act
            var health =  await controller.GetTrial("1");
            var result = health as OkObjectResult;
          
            // Assert
            trialService.Verify(r => r.GetTrialAsync("1",false));
            Assert.Equal("Test", ((ClinicalTrialDto)result.Value).Title);
        }
    }
}