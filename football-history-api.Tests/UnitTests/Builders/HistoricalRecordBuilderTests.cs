using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Bindings;
using football.history.api.Builders;
using football.history.api.Domain;
using football.history.api.Exceptions;
using football.history.api.Repositories;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.UnitTests.Builders
{
    [TestFixture]
    public class HistoricalRecordBuilderTests
    {
        [Test]
        public void Build_returns_empty_historical_record_given_no_historical_seasons()
        {
            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(new List<HistoricalSeasonModel>());

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var (teamId, historicalSeasons) = builder.Build(1, Array.Empty<long>());

            Assert.That(teamId, Is.EqualTo(1));
            Assert.That(historicalSeasons, Is.Empty);
        }

        [Test]
        public void Build_throws_exception_given_a_team_with_positions_in_multiple_competitions_in_a_season()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 1,
                        CompetitionName: "",
                        Tier: 1,
                        TotalPlaces: 1,
                        Position: 1,
                        Status: null)),
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 2,
                        CompetitionName: "",
                        Tier: 2,
                        TotalPlaces: 1,
                        Position: 1,
                        Status: null))
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var ex = Assert.Throws<DataInvalidException>(() => builder.Build(1, new[] {1L}));

            Assert.That(ex.Message,
                Is.EqualTo("The team was assigned positions in more than one competition in a season."));
        }

        [Test]
        public void Build_throws_exception_given_season_with_missing_competitions()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 2000,
                    PositionModel: null),
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 1,
                        CompetitionName: "",
                        Tier: 1,
                        TotalPlaces: 1,
                        Position: 1,
                        Status: null))
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var ex = Assert.Throws<DataInvalidException>(() => builder.Build(1, new[] {1L}));

            Assert.That(ex.Message, Does.Contain("Some competitions were null in the given season"));
            Assert.That(ex.Message, Does.Contain("(id: 1, startYear: 2000)."));
        }

        [Test]
        public void Build_returns_record_with_no_boundaries_and_null_position_given_seasons_without_competitions()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 1940,
                    PositionModel: null),
                new(SeasonId: 2,
                    StartYear: 1941,
                    PositionModel: null)
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var (_, historicalSeasons) = builder.Build(1, new[] {1L});

            Assert.That(historicalSeasons.Length, Is.EqualTo(2));

            var seasonOne = historicalSeasons.Single(x => x.SeasonId == 1);
            Assert.That(seasonOne.HistoricalPosition, Is.Null);
            Assert.That(seasonOne.Boundaries, Is.Empty);

            var seasonTwo = historicalSeasons.Single(x => x.SeasonId == 2);
            Assert.That(seasonTwo.HistoricalPosition, Is.Null);
            Assert.That(seasonTwo.Boundaries, Is.Empty);
        }

        [Test]
        public void Build_returns_record_with_boundaries_and_no_position_given_team_not_in_competitions()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 1,
                        CompetitionName: "",
                        Tier: 1,
                        TotalPlaces: 20,
                        Position: null,
                        Status: null))
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var (_, historicalSeasons) = builder.Build(1, new[] {1L});

            Assert.That(historicalSeasons.Length, Is.EqualTo(1));

            var season = historicalSeasons.Single();
            Assert.That(season.HistoricalPosition, Is.Null);
            Assert.That(season.Boundaries, Is.EquivalentTo(new[] {20}));
        }

        [Test]
        public void Build_returns_record_with_correct_historical_position()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 1,
                        CompetitionName: "First Division",
                        Tier: 1,
                        TotalPlaces: 20,
                        Position: null,
                        Status: null)),
                new(SeasonId: 1,
                    StartYear: 2000,
                    new HistoricalPositionModel(
                        CompetitionId: 2,
                        CompetitionName: "Second Division",
                        Tier: 2,
                        TotalPlaces: 24,
                        Position: 12,
                        Status: "status"))
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var (_, historicalSeasons) = builder.Build(1, new[] {1L});

            Assert.That(historicalSeasons.Length, Is.EqualTo(1));
            
            var season = historicalSeasons.Single();
            var expectedHistoricalPosition = new HistoricalPosition(
                CompetitionId: 2,
                CompetitionName: "Second Division", 
                Position: 12,
                OverallPosition: 32,
                Status: "status");
            
            Assert.That(season.HistoricalPosition, Is.EqualTo(expectedHistoricalPosition));
            Assert.That(season.Boundaries, Is.EquivalentTo(new[] {20, 44}));
        }
        
        [Test]
        public void Build_returns_record_with_correct_boundaries_given_north_south_divisions()
        {
            var models = new List<HistoricalSeasonModel>
            {
                new(SeasonId: 1,
                    StartYear: 1950,
                    new HistoricalPositionModel(
                        CompetitionId: 1,
                        CompetitionName: "First Division",
                        Tier: 1,
                        TotalPlaces: 20,
                        Position: null,
                        Status: null)),
                new(SeasonId: 1,
                    StartYear: 1950,
                    new HistoricalPositionModel(
                        CompetitionId: 2,
                        CompetitionName: "Second Division",
                        Tier: 2,
                        TotalPlaces: 24,
                        Position: 12,
                        Status: null)),
                new(SeasonId: 1,
                    StartYear: 1950,
                    new HistoricalPositionModel(
                        CompetitionId: 3,
                        CompetitionName: "Third Division North",
                        Tier: 3,
                        TotalPlaces: 24,
                        Position: null,
                        Status: null)),
                new(SeasonId: 1,
                    StartYear: 1950,
                    new HistoricalPositionModel(
                        CompetitionId: 4,
                        CompetitionName: "Third Division South",
                        Tier: 3,
                        TotalPlaces: 24,
                        Position: null,
                        Status: null))
            };

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Returns(models);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var (_, historicalSeasons) = builder.Build(1, new[] {1L});

            Assert.That(historicalSeasons.Length, Is.EqualTo(1));
            Assert.That(historicalSeasons.Single().Boundaries, Is.EquivalentTo(new[] {20, 44, 68, 68}));
        }
        
        [Test]
        public void Build_passes_repository_exception_to_caller()
        {
            var exception = new Exception("Repository exception");

            var mockHistoricalSeasonRepository = new Mock<IHistoricalSeasonRepository>();
            mockHistoricalSeasonRepository
                .Setup(x => x.GetHistoricalSeasons(It.IsAny<long>(), It.IsAny<long[]>()))
                .Throws(exception);

            var builder = new HistoricalRecordBuilder(mockHistoricalSeasonRepository.Object);

            var ex =Assert.Throws<Exception>(() => builder.Build(1, new[] { 1L }));

            Assert.That(ex, Is.EqualTo(exception));
        }
    }
}