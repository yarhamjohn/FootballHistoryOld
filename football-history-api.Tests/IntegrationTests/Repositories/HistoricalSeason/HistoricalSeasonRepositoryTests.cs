using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using football.history.api.Repositories;
using football.history.api.Repositories.Match;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;

namespace football.history.api.Tests.IntegrationTests.Repositories.HistoricalSeason
{
    [TestFixture]
    public class HistoricalSeasonRepositoryTests
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
        public void GetHistoricalSeasons_returns_empty_list_given_no_matching_teamId()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(0, new long[] { 1, 2, 3 });
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_empty_list_given_no_seasonIds()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(1, Array.Empty<long>());
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_empty_list_given_no_matching_seasonIds()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(1, new long[] { 0, 99 });
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_season_with_null_position_model_given_season_with_no_competition()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(1, new long[] { 3 });
            
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Single().SeasonId, Is.EqualTo(3));
            Assert.That(result.Single().PositionModel, Is.Null);
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_season_with_all_null_positions_if_team_exists_but_not_in_competitions()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(21, new long[] { 1, 2 });
            
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result.Select(x => x.SeasonId), Is.EquivalentTo(new long[] {1, 2, 2}));
            Assert.That(result.Select(x => x.StartYear), Is.EquivalentTo(new long[] {2000, 2001, 2001}));
            Assert.That(result.Select(x => x.PositionModel), Is.EquivalentTo(new []
            {
                new HistoricalPositionModel(1, "First Division", 1, 10, null, null),
                new HistoricalPositionModel(2, "First Division", 1, 10, null, null),
                new HistoricalPositionModel(3, "Second Division", 2, 10, null, null)
            }));
        }
        
        [Test]
        public void GetHistoricalSeasons_returns_correct_seasons()
        {
            var repository = new HistoricalSeasonRepository(_testDbConnection);
            
            var result = repository.GetHistoricalSeasons(1, new long[] { 1, 2 });
            
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result.Select(x => x.SeasonId), Is.EquivalentTo(new long[] {1, 2, 2}));
            Assert.That(result.Select(x => x.StartYear), Is.EquivalentTo(new long[] {2000, 2001, 2001}));
            Assert.That(result.Select(x => x.PositionModel), Is.EquivalentTo(new []
            {
                new HistoricalPositionModel(1, "First Division", 1, 10, 1, "Champions"),
                new HistoricalPositionModel(2, "First Division", 1, 10, 1, "Champions"),
                new HistoricalPositionModel(3, "Second Division", 2, 10, null, null)
            }));
        }
    }

    public class TestDatabaseConnection : IDatabaseConnection
    {
        private readonly SqlConnection _conn;
        private readonly string _databaseName;

        public TestDatabaseConnection(string databaseName)
        {
            _databaseName = databaseName;
            _conn = new SqlConnection("Server=localhost;User=sa;Password=2@LaiNw)PDvs^t>L!Ybt]6H^%h3U>M");
        }
        
        public void Open()
        {
            try
            {
                _conn.Open();
                _conn.ChangeDatabase(_databaseName);
            }
            catch (InvalidOperationException)
            {
                Close();
                Open();
            }
        }

        public void Close()
        {
            _conn.Close();
        }

        public DbCommand CreateCommand()
        {
            return _conn.CreateCommand();
        }

        public void CreateDatabase()
        {
            _conn.Open();

            CreateTestDatabase();
            PopulateTestDatabase();
            
            Close();
        }

        private void PopulateTestDatabase()
        {
            _conn.ChangeDatabase(_databaseName);

            var script = File.ReadAllText(@"C:\repos\football-history\football-history-api.Tests\Data.sql");
            var server = new Server(new ServerConnection(_conn));
            server.ConnectionContext.ExecuteNonQuery(script);
        }

        private void CreateTestDatabase()
        {
            DropTestDatabaseIfExists();

            var cmd = CreateCommand();
            cmd.CommandText = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                    BEGIN
                        CREATE DATABASE {_databaseName};
                    END;";
            cmd.ExecuteNonQuery();
        }

        public void DropDatabase()
        {
            _conn.Open();

            DropTestDatabaseIfExists();

            _conn.Close();
        }

        private void DropTestDatabaseIfExists()
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $@"
                    IF EXISTS (SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                    BEGIN
                        DROP DATABASE {_databaseName};
                    END;";
            cmd.ExecuteNonQuery();
        }
    }
}