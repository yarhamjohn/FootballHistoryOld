using System.Linq;
using football.history.api.Repositories.Season;
using football.history.api.Tests.IntegrationTests.Repositories.TestUtilities;
using NUnit.Framework;

namespace football.history.api.Tests.IntegrationTests.Repositories
{
    [TestFixture]
    public class SeasonRepositoryTests
    {
        private readonly TestDatabaseConnection _testDbConnection = new("testDatabase");

        [SetUp]
        public void SetUp()
        {
            _testDbConnection.CreateDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _testDbConnection.DropDatabase();
        }
        
        [Test]
        public void GetAllSeasons_returns_all_seasons()
        {
            var repository = new SeasonRepository(_testDbConnection);
            
            var result = repository.GetAllSeasons();
            
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.Single(x => x.Id is 1).StartYear, Is.EqualTo(2000));
        }
        
        [Test]
        public void GetSeason_returns_null_given_no_matching_seasonId()
        {
            var repository = new SeasonRepository(_testDbConnection);
            
            var result = repository.GetSeason(0);
            
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetSeason_returns_season_model_given_matching_seasonId()
        {
            var repository = new SeasonRepository(_testDbConnection);
            
            var result = repository.GetSeason(1);

            var expectedModel = new SeasonModel(1, 2000, 2001);
            Assert.That(result, Is.EqualTo(expectedModel));
        }
    }
}