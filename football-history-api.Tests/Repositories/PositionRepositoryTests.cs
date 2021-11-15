using System.Linq;
using football.history.api.Repositories;
using football.history.api.Tests.TestUtilities;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories;

[TestFixture]
public class PositionRepositoryTests
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
    public void GetTeamPositions_returns_empty_given_non_matching_team_id()
    {
        var repository = new PositionRepository(_testDbConnection);
            
        var result = repository.GetTeamPositions(0);
        
        Assert.That(result, Is.Empty);
    }
        
    [Test]
    public void GetTeamPositions_returns_empty_given_team_with_no_positions()
    {
        var repository = new PositionRepository(_testDbConnection);
            
        var result = repository.GetTeamPositions(21);
        
        Assert.That(result, Is.Empty);
    }
        
    [Test]
    public void GetTeamPositions_returns_data_given_matching_team_id()
    {
        var repository = new PositionRepository(_testDbConnection);
            
        var result = repository.GetTeamPositions(1);
        
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Select(x => x.LeaguePosition), Is.EquivalentTo(new[] { 1, 1 }));
        Assert.That(result.Select(x => x.CompetitionId), Is.EquivalentTo(new[] { 1, 2 }));
        Assert.That(result.Select(x => x.Status), Is.EquivalentTo(new[] { "Champions", "Champions" }));
    }
        
    [Test]
    public void GetCompetitionPositions_returns_empty_given_non_matching_competition_id()
    {
        var repository = new PositionRepository(_testDbConnection);
            
        var result = repository.GetCompetitionPositions(0);
        
        Assert.That(result, Is.Empty);
    }
        
    [Test]
    public void GetCompetitionPositions_returns_data_given_matching_competition_id()
    {
        var repository = new PositionRepository(_testDbConnection);
            
        var result = repository.GetCompetitionPositions(1);
        
        Assert.That(result.Count, Is.EqualTo(10));
        Assert.That(result.Single(x => x.LeaguePosition == 1).Status, Is.EqualTo("Champions"));
    }
}