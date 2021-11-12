using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Models;
using football.history.api.Repositories;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.UnitTests.Builders.LeaguePositions;

[TestFixture]
public class LeaguePositionBuilderTests
{
    [Test]
    public void GetPositions_returns_empty_list_if_no_matches_in_competition()
    {
        var mockDirector = new Mock<ILeagueTableBuilder>();
        var mockMatchRepository = new Mock<IMatchRepository>();
        var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
        mockMatchRepository.Setup(x => x.GetLeagueMatches(1)).Returns(Array.Empty<MatchModel>());

        var builder = new LeaguePositionBuilder(mockDirector.Object, mockMatchRepository.Object,
            mockPointDeductionRepository.Object);
        var competition = GetCompetitionModel();

        var positions = builder.GetPositions(1, competition);

        mockMatchRepository.VerifyAll();
        positions.Should().BeEmpty();
    }

    [Test]
    public void GetPositions_returns_position_for_full_date_range()
    {
        var competition = GetCompetitionModel();

        var mockLeagueTable = new Mock<ILeagueTable>();
        mockLeagueTable.Setup(x => x.GetPosition(It.IsAny<long>())).Returns(1);

        var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
        var pointDeductions = Array.Empty<PointDeductionModel>();
        mockPointDeductionRepository.Setup(x => x.GetPointDeductions(competition.Id))
            .Returns(pointDeductions);

        var mockMatchRepository = new Mock<IMatchRepository>();
        var matches = GetMatches();
        mockMatchRepository
            .Setup(x => x.GetLeagueMatches(1))
            .Returns(matches);

        var mockDirector = new Mock<ILeagueTableBuilder>();
        mockDirector
            .Setup(x => x.BuildPartialLeagueTable(
                competition,
                matches,
                It.IsAny<DateTime>(),
                pointDeductions))
            .Returns(mockLeagueTable.Object);

        var builder = new LeaguePositionBuilder(mockDirector.Object, mockMatchRepository.Object,
            mockPointDeductionRepository.Object);

        var positions = builder.GetPositions(1, competition);

        mockDirector.Verify(x =>
            x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 9), pointDeductions), Times.Once);
        mockDirector.Verify(x =>
                x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 10), pointDeductions),
            Times.Once);
        mockDirector.Verify(x =>
                x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 11), pointDeductions),
            Times.Once);
        mockDirector.Verify(x =>
                x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 12), pointDeductions),
            Times.Once);
        mockDirector.Verify(x =>
                x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 13), pointDeductions),
            Times.Once);
        mockDirector.Verify(x =>
                x.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 14), pointDeductions),
            Times.Once);

        positions.Should().HaveCount(6);
        positions.Should().BeEquivalentTo(new List<LeaguePositionDto>
        {
            new(new DateTime(2000, 1, 9), 1),
            new(new DateTime(2000, 1, 10), 1),
            new(new DateTime(2000, 1, 11), 1),
            new(new DateTime(2000, 1, 12), 1),
            new(new DateTime(2000, 1, 13), 1),
            new(new DateTime(2000, 1, 14), 1)
        });
    }

    private static CompetitionModel GetCompetitionModel()
    {
        return new(
            Id: 1,
            Name: "Premier League",
            SeasonId: 1,
            StartYear: 2000,
            EndYear: 2001,
            Tier: 1,
            Region: null,
            Comment: null,
            PointsForWin: 3,
            TotalPlaces: 20,
            PromotionPlaces: 0,
            RelegationPlaces: 3,
            PlayOffPlaces: 0,
            RelegationPlayOffPlaces: 0,
            ReElectionPlaces: 0,
            FailedReElectionPosition: null);
    }

    private static MatchModel[] GetMatches()
    {
        return new MatchModel[]
        {
            new(1,
                new DateTime(2000, 1, 10),
                1,
                "Premier League",
                2000,
                2001,
                1,
                null,
                "League",
                null,
                false,
                false,
                null,
                false,
                false,
                1,
                "Norwich City",
                "NOR",
                2,
                "Newcastle United",
                "NEW",
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0),
            new(2,
                new DateTime(2000, 1, 12),
                1,
                "Premier League",
                2000,
                2001,
                1,
                null,
                "League",
                null,
                false,
                false,
                null,
                false,
                false,
                2,
                "Newcastle United",
                "NEW",
                3,
                "Arsenal",
                "Ars",
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0),
            new(3,
                new DateTime(2000, 1, 13),
                1,
                "Premier League",
                2000,
                2001,
                1,
                null,
                "League",
                null,
                false,
                false,
                null,
                false,
                false,
                2,
                "Newcastle United",
                "NEW",
                3,
                "Arsenal",
                "Ars",
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0)
        };
    }
}