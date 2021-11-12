using System.Linq;
using football.history.api.Repositories;
using football.history.api.Tests.IntegrationTests.Repositories.TestUtilities;
using NUnit.Framework;

namespace football.history.api.Tests.IntegrationTests.Repositories;

[TestFixture]
public class PointDeductionRepositoryTests
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
    public void GetPointDeductions_returns_empty_list_given_no_data()
    {
        var repository = new PointDeductionRepository(_testDbConnection);

        var result = repository.GetPointDeductions(0);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPointDeductions_returns_empty_list_given_no_matching_competition_id()
    {
        var repository = new PointDeductionRepository(_testDbConnection);

        var result = repository.GetPointDeductions(2);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPointDeductions_returns_matching_data()
    {
        var repository = new PointDeductionRepository(_testDbConnection);

        var result = repository.GetPointDeductions(1);
        
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Select(pd => pd.PointsDeducted), 
            Is.EquivalentTo(new[] { 9, 12 }));
        Assert.That(result.Select(pd => pd.TeamName), 
            Is.EquivalentTo(new[] { "Accrington Stanley", "AFC Bournemouth" }));
    }
}