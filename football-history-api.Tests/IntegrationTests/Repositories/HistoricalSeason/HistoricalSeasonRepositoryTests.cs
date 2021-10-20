using System;
using NUnit.Framework;

namespace football.history.api.Tests.IntegrationTests.Repositories.HistoricalSeason
{
    [TestFixture]
    public class HistoricalSeasonRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            // Create test database with data inside docker container
        }

        [TearDown]
        public void TearDown()
        {
            // Clear test database
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_empty_list_given_no_matching_teamId()
        {
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_empty_list_given_no_seasonIds()
        {
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_empty_list_given_no_matching_seasonIds()
        {
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_season_with_null_position_model_given_season_with_no_competition()
        {
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_correct_seasons()
        {
        }
    }
}