namespace football.history.api.Tests.Repositories;

[TestFixture]
public class TeamRepositoryTests
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
    public void GetAllTeams_returns_all_teams()
    {
        var repository = new TeamRepository(_testDbConnection);
            
        var result = repository.GetAllTeams();
            
        Assert.That(result.Count, Is.EqualTo(21));
        Assert.That(result.Single(x => x.Id is 1).Name, Is.EqualTo("Accrington Stanley"));
    }
        
    [Test]
    public void GetTeam_returns_null_given_no_matching_teamId()
    {
        var repository = new TeamRepository(_testDbConnection);
            
        var result = repository.GetTeam(0);
            
        Assert.That(result, Is.Null);
    }
        
    [Test]
    public void GetTeam_returns_team_model_given_matching_teamId()
    {
        var repository = new TeamRepository(_testDbConnection);
            
        var result = repository.GetTeam(1);

        var expectedModel = new TeamModel(1, "Accrington Stanley", "ACC", null);
        Assert.That(result, Is.EqualTo(expectedModel));
    }
}