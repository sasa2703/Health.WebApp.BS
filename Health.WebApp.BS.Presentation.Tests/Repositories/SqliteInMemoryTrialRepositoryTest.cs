using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Repository;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace HealthManager.WebApp.BS.Presentation.Tests.Repositories
{
    public class SqliteInMemoryTrialRepositoryTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<RepositoryContext> _contextOptions;

        #region ConstructorAndDispose
        public SqliteInMemoryTrialRepositoryTest()
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
FROM Trial;"
                ;
                viewCommand.ExecuteNonQuery();
            }
            
            context.SaveChanges();



            context.AddRange(
                new ClinicalTrialMetadata { Title = "Andol", TrialId = "1", StartDate = DateTime.Now.AddDays(-1), Status = "NotStarted" },
                new ClinicalTrialMetadata { Title = "Vitamin C", TrialId = "2", StartDate = DateTime.Now.AddDays(-1), Status = "NotStarted" });
            context.SaveChanges();
        }

        RepositoryContext CreateContext() => new RepositoryContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion


        [Fact]
        public async void GetTrial_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);

            //Act
            var trial = await trialRepository.GetTrialAsync("1", false);

            //Assert
            Assert.Equal("Andol", trial.Title);
        }

        [Fact]
        public async void GetAllTrial_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);
            var trialParameters = new TrialParameters() { OnlyAvailable = true, PageNumber = 0, PageSize = 10 };

            // Act
            var trial = await trialRepository.GetAllTrialsAsync(trialParameters, false);

            // Assert
            Assert.Collection(
           trial,
           b => Assert.Equal("Andol", b.Title),
           b => Assert.Equal("Vitamin C", b.Title));
        }

        [Fact]
        public void AddHTrial_Returns_OK()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);
            var trial = new ClinicalTrialMetadata() { TrialId = "3", Title = "Vitamin B", StartDate = DateTime.Now.AddDays(-2), Status = "NotStarted" };

            //Act
            trialRepository.CreateTrial(trial);
            context.SaveChanges();

            //Assert
            Assert.Equal("Vitamin B", trial.Title);
        }

        [Fact]
        public async void AddTrialWithoutStatus_Returns_DbUpdateException()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);
            var trial = new ClinicalTrialMetadata() { TrialId = "3", Title = "Vitamin B", StartDate = DateTime.Now.AddDays(-2) };


            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                trialRepository.CreateTrial(trial);
                await context.SaveChangesAsync(); // This line should throw the exception
            });

            // Assert the error message
            Assert.Contains("Model validation failed: The Status field is required.", exception.Message);
        }

        [Fact]
        public async void AddTrialWithtWrongStatus_Returns_DbUpdateException()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);
            var trial = new ClinicalTrialMetadata() { TrialId = "3", Title = "Vitamin B", StartDate = DateTime.Now.AddDays(-2), Status = "XXX" };


            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                trialRepository.CreateTrial(trial);
                await context.SaveChangesAsync(); // This line should throw the exception
            });

            // Assert the error message
            Assert.Contains("Model validation failed: Invalid status value. Allowed values: NotStarted, Ongoing, Completed.", exception.Message);
        }

        [Fact]
        public async void UpdateTrial_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);

            // Act
            var trialToUpdate = await trialRepository.GetTrialAsync("1", false);
            trialToUpdate.Title = "Vitamin D";
            trialRepository.UpdateTrial(trialToUpdate);
            context.SaveChanges();


            //Assert
            var health = context.ClinicalTrial.Single(b => b.TrialId == "1");
            Assert.Equal("Vitamin D", health.Title);
        }

        [Fact]
        public async void DeleteTrial_Return_OK()
        {
            // Arrange
            using var context = CreateContext();
            var trialRepository = new TrialRepository(context);

            //Act
            await trialRepository.DeleteTrialAsync("1");
            context.SaveChanges();

            //Assert
            var trial = context.ClinicalTrial.SingleOrDefault(b => b.TrialId == "1");
            Assert.Equal(null, trial);

        }
    }
}
