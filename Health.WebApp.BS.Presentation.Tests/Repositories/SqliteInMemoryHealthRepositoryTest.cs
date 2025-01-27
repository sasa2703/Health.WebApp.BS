using AutoMapper;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Repository;
using HealthManager.WebApp.BS.Service;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Common;

namespace HealthManager.WebApp.BS.Presentation.Tests.Repositories
{
    public class SqliteInMemoryHealthRepositoryTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<RepositoryContext> _contextOptions;

        #region ConstructorAndDispose
        public SqliteInMemoryHealthRepositoryTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<RepositoryContext>().UseSqlite(_connection).Options;


            // Create the schema and seed some data
            using var context = new RepositoryContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT Url
FROM Health;"
                ;
                viewCommand.ExecuteNonQuery();
            }
            
            context.SaveChanges();



            context.AddRange(
                new ClinicalTrialMetadata { Title = "Andol", TrialId = "1", StartDate = DateTime.Now.AddDays(-1), Status = "Not Started" },
                new ClinicalTrialMetadata { Title = "Vitamin C", TrialId = "2", StartDate = DateTime.Now.AddDays(-1), Status = "Not Started" });
            context.SaveChanges();
        }

        RepositoryContext CreateContext() => new RepositoryContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion


        [Fact]
        public async void GetHealth_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var healthRepository = new TrialRepository(context);

            //Act
            var health = await healthRepository.GetTrialAsync("1", false);

            //Assert
            Assert.Equal("Andol", health.Title);
        }

        [Fact]
        public async void GetAllHealth_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var healthRepository = new TrialRepository(context);
            var healthParameters = new TrialParameters() { OnlyAvailable = true, PageNumber = 0, PageSize = 10 };

            // Act
            var health = await healthRepository.GetAllTrialsAsync(healthParameters, false);

            // Assert
            Assert.Collection(
           health,
           b => Assert.Equal("Andol", b.Title),
           b => Assert.Equal("Vitamin C", b.Title));
        }

        [Fact]
        public void AddHealth_Returns_OK()
        {
            // Arrange
            using var context = CreateContext();
            var healthRepository = new TrialRepository(context);
            var health = new ClinicalTrialMetadata() { TrialId = "3", Title = "Vitamin B", StartDate = DateTime.Now.AddDays(-2), Status = "Not Started" };

            //Act
            healthRepository.CreateTrial(health);
            context.SaveChanges();

            //Assert
            Assert.Equal("Vitamin B", health.Title);
        }

        [Fact]
        public async void UpdateHealth_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var healthRepository = new TrialRepository(context);

            // Act
            var healthToUpdate = await healthRepository.GetTrialAsync("1", false);
            healthToUpdate.Title = "Vitamin D";
            healthRepository.UpdateTrial(healthToUpdate);
            context.SaveChanges();


            //Assert
            var health = context.ClinicalTrial.Single(b => b.TrialId == "1");
            Assert.Equal("Vitamin D", health.Title);
        }

        [Fact]
        public async void DeleteHealth_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var healthRepository = new TrialRepository(context);

            //Act
            await healthRepository.DeleteTrialAsync("1");
            context.SaveChanges();

            //Assert
            var health = context.ClinicalTrial.SingleOrDefault(b => b.TrialId == "1");
            Assert.Equal(null, health);

        }
    }
}
