using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Models;
using football.history.api.Repositories;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Builders;

[TestFixture]
public class CompetitionBuilderTests
{
    [Test]
    public void BuildCompetitions_returns_empty_array_given_no_competitions()
    {
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetitions(It.IsAny<long?>()))
            .Returns(Array.Empty<CompetitionModel>());

        var builder = new CompetitionBuilder(mockCompetitionRepository.Object);

        var competitions = builder.BuildCompetitions();

        Assert.That(competitions, Is.Empty);
    }

    [TestCase(null)]
    [TestCase(1)]
    public void BuildCompetitions_returns_domain_objects_given_models(long? seasonId)
    {
        var models = new[]
        {
            new CompetitionModel(
                Id: 1,
                Name: "First Division",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 1,
                Region: null,
                Comment: null,
                PointsForWin: 1,
                TotalPlaces: 1,
                PromotionPlaces: 1,
                RelegationPlaces: 1,
                PlayOffPlaces: 1,
                RelegationPlayOffPlaces: 1,
                ReElectionPlaces: 1,
                FailedReElectionPosition: null)
        };
            
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetitions(seasonId))
            .Returns(models);

        var builder = new CompetitionBuilder(mockCompetitionRepository.Object);

        var competitions = builder.BuildCompetitions(seasonId);

        Assert.That(competitions.Count, Is.EqualTo(1));

        var actualCompetition = competitions.Single();
        var expectedCompetitionModel = models.Single();
            
        Assert.That(actualCompetition.Id, Is.EqualTo(expectedCompetitionModel.Id));
        Assert.That(actualCompetition.Name, Is.EqualTo(expectedCompetitionModel.Name));
        Assert.That(actualCompetition.Season.StartYear, Is.EqualTo(expectedCompetitionModel.StartYear));
        Assert.That(actualCompetition.Rules.PointsForWin, Is.EqualTo(expectedCompetitionModel.PointsForWin));
    }

    [Test]
    public void BuildCompetition_returns_null_given_null_model()
    {
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(It.IsAny<long>()))
            .Returns((CompetitionModel?) null);

        var builder = new CompetitionBuilder(mockCompetitionRepository.Object);

        var competition = builder.BuildCompetition(1);

        Assert.That(competition, Is.Null);
    }
        
    [Test]
    public void BuildCompetition_does_not_handle_repository_exception()
    {
        var exception = new DataInvalidException("An error occurred");

        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(It.IsAny<long>()))
            .Throws(exception);

        var builder = new CompetitionBuilder(mockCompetitionRepository.Object);

        var ex = Assert.Throws<DataInvalidException>(() => builder.BuildCompetition(1));

        Assert.That(ex, Is.EqualTo(exception));
    }

    [Test]
    public void BuildCompetition_returns_domain_object_given_model()
    {
        var expectedCompetitionModel = new CompetitionModel(
            Id: 1,
            Name: "First Division",
            SeasonId: 1,
            StartYear: 2000,
            EndYear: 2001,
            Tier: 1,
            Region: null,
            Comment: null,
            PointsForWin: 1,
            TotalPlaces: 1,
            PromotionPlaces: 1,
            RelegationPlaces: 1,
            PlayOffPlaces: 1,
            RelegationPlayOffPlaces: 1,
            ReElectionPlaces: 1,
            FailedReElectionPosition: null);
            
        var mockCompetitionRepository = new Mock<ICompetitionRepository>();
        mockCompetitionRepository
            .Setup(x => x.GetCompetition(1))
            .Returns(expectedCompetitionModel);

        var builder = new CompetitionBuilder(mockCompetitionRepository.Object);

        var actualCompetition = builder.BuildCompetition(1);

        Assert.That(actualCompetition, Is.Not.Null);

        Assert.That(actualCompetition!.Id, Is.EqualTo(expectedCompetitionModel.Id));
        Assert.That(actualCompetition.Name, Is.EqualTo(expectedCompetitionModel.Name));
        Assert.That(actualCompetition.Season.StartYear, Is.EqualTo(expectedCompetitionModel.StartYear));
        Assert.That(actualCompetition.Rules.PointsForWin, Is.EqualTo(expectedCompetitionModel.PointsForWin));
    }
}