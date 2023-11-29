namespace football.history.api.Tests.Repositories;

[TestFixture]
public class CompetitionRepositoryTests
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
    public void GetCompetitions_returns_all_competitions_given_no_season_id()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetitions();

        Assert.That(result.Length, Is.EqualTo(3));
    }

    [Test]
    public void GetCompetitions_returns_empty_list_given_missing_season()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetitions(0);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetCompetitions_returns_empty_list_given_season_with_no_competitions()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetitions(3);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetCompetitions_returns_competitions_in_season()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetitions(2);

        Assert.That(result.Length, Is.EqualTo(2));
        Assert.That(result.Select(x => x.Name), Is.EquivalentTo(new[] { "First Division", "Second Division" }));
        Assert.That(result.Select(x => x.Tier), Is.EquivalentTo(new[] { 1, 2 }));
    }

    [Test]
    public void GetCompetition_returns_null_given_no_competitions_matching_competition_id()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetition(0);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCompetition_returns_competition_given_matching_competition_id()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetCompetition(1);

        Assert.That(result, Is.Not.Null);
        
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("First Division"));
        Assert.That(result.SeasonId, Is.EqualTo(1));
        Assert.That(result.Tier, Is.EqualTo(1));
    }

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    public void GetTierCompetition_returns_null_given_missing_season_or_tier(long seasonId, int tier)
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTierCompetition(seasonId, tier);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTierCompetition_returns_null_given_no_matching_season_and_tier()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTierCompetition(1, 2);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTierCompetition_returns_competition_given_matching_season_and_tier()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTierCompetition(1, 1);

        Assert.That(result, Is.Not.Null);
        
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("First Division"));
        Assert.That(result.SeasonId, Is.EqualTo(1));
        Assert.That(result.Tier, Is.EqualTo(1));
    }

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    public void GetTeamCompetition_returns_null_given_missing_season_or_team(long seasonId, long teamId)
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTeamCompetition(0, 1);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTeamCompetition_returns_null_given_no_matching_season_and_team()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTeamCompetition(1, 21);

        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void GetTeamCompetition_returns_competition_given_matching_season_and_team()
    {
        var repository = new CompetitionRepository(_testDbConnection);

        var result = repository.GetTeamCompetition(2, 20);

        Assert.That(result, Is.Not.Null);
        
        Assert.That(result!.Id, Is.EqualTo(3));
        Assert.That(result.Name, Is.EqualTo("Second Division"));
        Assert.That(result.SeasonId, Is.EqualTo(2));
        Assert.That(result.Tier, Is.EqualTo(2));
    }
}