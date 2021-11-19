using football.history.api.Builders;
using football.history.api.Models;
using football.history.api.Repositories;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Builders;

[TestFixture]
public class TeamBuilderTests
{
    [Test]
    public void BuildAllTeams_returns_empty_array_given_no_teams()
    {
        var mockTeamRepository = new Mock<ITeamRepository>();
        mockTeamRepository
            .Setup(x => x.GetAllTeams())
            .Returns(Array.Empty<TeamModel>());

        var builder = new TeamBuilder(mockTeamRepository.Object);

        var teams = builder.BuildAllTeams();

        Assert.That(teams, Is.Empty);
    }
        
    [Test]
    public void BuildAllTeams_returns_domain_objects_given_models()
    {
        var models = new[]
        {
            new TeamModel(Id: 1, Name: "Norwich City", Abbreviation: "NOR", Notes: null)
        };
            
        var mockTeamRepository = new Mock<ITeamRepository>();
        mockTeamRepository
            .Setup(x => x.GetAllTeams())
            .Returns(models);

        var builder = new TeamBuilder(mockTeamRepository.Object);

        var teams = builder.BuildAllTeams();

        Assert.That(teams.Count, Is.EqualTo(1));

        var actualTeam = teams.Single();
        var expectedTeamModel = models.Single();
            
        Assert.That(actualTeam.Id, Is.EqualTo(expectedTeamModel.Id));
        Assert.That(actualTeam.Name, Is.EqualTo(expectedTeamModel.Name));
        Assert.That(actualTeam.Abbreviation, Is.EqualTo(expectedTeamModel.Abbreviation));
        Assert.That(actualTeam.Notes, Is.EqualTo(expectedTeamModel.Notes));
    }
        
    [Test]
    public void BuildTeam_returns_null_given_null_model()
    {
        var mockTeamRepository = new Mock<ITeamRepository>();
        mockTeamRepository
            .Setup(x => x.GetTeam(It.IsAny<long>()))
            .Returns((TeamModel?) null);

        var builder = new TeamBuilder(mockTeamRepository.Object);

        var team = builder.BuildTeam(1);

        Assert.That(team, Is.Null);
    }
        
    [Test]
    public void BuildTeam_does_not_handle_repository_exception()
    {
        var exception = new InvalidOperationException("An error occurred");

        var mockTeamRepository = new Mock<ITeamRepository>();
        mockTeamRepository
            .Setup(x => x.GetTeam(It.IsAny<long>()))
            .Throws(exception);

        var builder = new TeamBuilder(mockTeamRepository.Object);

        var ex = Assert.Throws<InvalidOperationException>(() => builder.BuildTeam(1));

        Assert.That(ex, Is.EqualTo(exception));
    }

    [Test]
    public void BuildTeam_returns_domain_object_given_model()
    {
        var expectedTeamModel = new TeamModel(Id: 1, Name: "Norwich City", Abbreviation: "NOR", Notes: null);
            
        var mockTeamRepository = new Mock<ITeamRepository>();
        mockTeamRepository
            .Setup(x => x.GetTeam(1))
            .Returns(expectedTeamModel);

        var builder = new TeamBuilder(mockTeamRepository.Object);

        var actualTeam = builder.BuildTeam(1);

        Assert.That(actualTeam, Is.Not.Null);

        Assert.That(actualTeam!.Id, Is.EqualTo(expectedTeamModel.Id));
        Assert.That(actualTeam.Name, Is.EqualTo(expectedTeamModel.Name));
        Assert.That(actualTeam.Abbreviation, Is.EqualTo(expectedTeamModel.Abbreviation));
        Assert.That(actualTeam.Notes, Is.EqualTo(expectedTeamModel.Notes));
    }
}