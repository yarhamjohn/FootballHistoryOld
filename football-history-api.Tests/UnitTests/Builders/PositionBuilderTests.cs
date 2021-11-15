using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.UnitTests.Builders;

[TestFixture]
public class PositionBuilderTests
{
    [Test]
    public void BuildSeasonPositions_returns_empty_array_given_no_competition_models()
    {
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetitionsInSeason(It.IsAny<long>()))
            .Returns(Array.Empty<CompetitionModel>());
        
        var mockPositionRepository = new Mock<IPositionRepository>();
        
        var builder = new PositionBuilder(mockPositionRepository.Object, mockCompetitionRepository.Object);

        var actualPositions = builder.BuildSeasonPositions(1);

        Assert.That(actualPositions, Is.Empty);
    }
    
    [Test]
    public void BuildSeasonPositions_returns_empty_array_given_no_position_models()
    {
        var competitionModels = new[]
        {
            new CompetitionModel(1, "Premier League", 1, 2000, 2001, 1, null, null, 0, 0, 0, 0, 0, 0, 0, null)
        };

        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetitionsInSeason(It.IsAny<long>()))
            .Returns(competitionModels);
        
        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository
            .Setup(x => x.GetCompetitionPositions(It.IsAny<long>()))
            .Returns(Array.Empty<PositionModel>());
        
        var builder = new PositionBuilder(mockPositionRepository.Object, mockCompetitionRepository.Object);

        var actualPositions = builder.BuildSeasonPositions(1);

        Assert.That(actualPositions, Is.Empty);
    }

    [Test]
    public void BuildSeasonPositions_returns_domain_objects()
    {
        var competitionModels = new[]
        {
            new CompetitionModel(1, "Premier League", 1, 2000, 2001, 1, null, null, 0, 0, 0, 0, 0, 0, 0, null),
            new CompetitionModel(2, "Championship", 1, 2000, 2001, 2, null, null, 0, 0, 0, 0, 0, 0, 0, null),
        };

        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetitionsInSeason(1))
            .Returns(competitionModels);

        var expectedPositionModelsOne = new[]
        {
            new PositionModel(
                Id: 1,
                CompetitionId: 1,
                CompetitionName: "Premier League",
                TeamId: 1,
                TeamName: "Norwich City",
                LeaguePosition: 1,
                Status: "Champions")
        };

        var expectedPositionModelsTwo = new[]
        {
            new PositionModel(
                Id: 2,
                CompetitionId: 2,
                CompetitionName: "Championship",
                TeamId: 2,
                TeamName: "Newcastle United",
                LeaguePosition: 20,
                Status: "Relegation")
        };

        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository
            .Setup(x => x.GetCompetitionPositions(1))
            .Returns(expectedPositionModelsOne);

        mockPositionRepository
            .Setup(x => x.GetCompetitionPositions(2))
            .Returns(expectedPositionModelsTwo);

        var builder = new PositionBuilder(mockPositionRepository.Object, mockCompetitionRepository.Object);

        var actualPositions = builder.BuildSeasonPositions(1);

        Assert.That(actualPositions.Count, Is.EqualTo(2));

        Assert.That(actualPositions.Select(x => x.TeamName),
            Is.EquivalentTo(new[] { "Norwich City", "Newcastle United" }));
    }
    

    [Test]
    public void BuildTeamPositions_returns_empty_array_given_no_models()
    {
        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository
            .Setup(x => x.GetTeamPositions(It.IsAny<long>()))
            .Returns(Array.Empty<PositionModel>());
        
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        
        var builder = new PositionBuilder(mockPositionRepository.Object, mockCompetitionRepository.Object);

        var actualPositions = builder.BuildTeamPositions(1);

        Assert.That(actualPositions, Is.Empty);
    }
    
    [Test]
    public void BuildTeamPositions_returns_domain_objects()
    {
        var expectedPositionModels = new[]
        {
            new PositionModel(
                Id: 1,
                CompetitionId: 1,
                CompetitionName: "Premier League",
                TeamId: 1,
                TeamName: "Norwich City",
                LeaguePosition: 1,
                Status: "Champions")
        };
        
        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository
            .Setup(x => x.GetTeamPositions(1))
            .Returns(expectedPositionModels);
        
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        
        var builder = new PositionBuilder(mockPositionRepository.Object, mockCompetitionRepository.Object);

        var actualPositions = builder.BuildTeamPositions(1);

        Assert.That(actualPositions.Count, Is.EqualTo(1));

        var actualPosition = actualPositions.Single();
        var expectedPositionModel = expectedPositionModels.Single();
        
        Assert.That(actualPosition.Id, Is.EqualTo(expectedPositionModel.Id));
        Assert.That(actualPosition.CompetitionId, Is.EqualTo(expectedPositionModel.CompetitionId));
        Assert.That(actualPosition.CompetitionName, Is.EqualTo(expectedPositionModel.CompetitionName));
        Assert.That(actualPosition.TeamId, Is.EqualTo(expectedPositionModel.TeamId));
        Assert.That(actualPosition.TeamName, Is.EqualTo(expectedPositionModel.TeamName));
        Assert.That(actualPosition.LeaguePosition, Is.EqualTo(expectedPositionModel.LeaguePosition));
        Assert.That(actualPosition.Status, Is.EqualTo(expectedPositionModel.Status));
    }
}