using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Models;
using football.history.api.Repositories;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.UnitTests.Builders
{
    [TestFixture]
    public class MatchBuilderTests
    {
        [Test]
        public void BuildMatches_returns_empty_array_given_no_matches()
        {
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatches(null, null, null, null, null))
                .Returns(Array.Empty<MatchModel>());

            var builder = new MatchBuilder(mockMatchRepository.Object);

            var matches = builder.BuildMatches(null, null, null, null, null);

            Assert.That(matches, Is.Empty);
        }
        
        [TestCase(1, 2, 3, MatchType.League, "2000-01-01")]
        [TestCase(null, 2, 3, MatchType.League, "2000-01-01")]
        [TestCase(1, null, 3, MatchType.League, "2000-01-01")]
        [TestCase(1, 2, null, MatchType.League, "2000-01-01")]
        [TestCase(1, 2, 3, null, "2000-01-01")]
        [TestCase(1, 2, 3, MatchType.League, null)]
        [TestCase(null, null, null, null, null)]
        public void BuildMatches_returns_empty_array_given_no_matches(
            long? competitionId, long? seasonId, long? teamId, MatchType? type, DateTime? matchDate)
        {
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatches(competitionId, seasonId, teamId, type, matchDate))
                .Returns(Array.Empty<MatchModel>());

            var builder = new MatchBuilder(mockMatchRepository.Object);

            builder.BuildMatches(competitionId, seasonId, teamId, type, matchDate);

            mockMatchRepository.VerifyAll();
        }
        
        [Test]
        public void BuildMatches_returns_domain_objects_given_models()
        {
            var models = new[]
            {
                new MatchModel(
                    Id: 1,
                    MatchDate: new DateTime(2000, 1, 1),
                    CompetitionId: 1,
                    CompetitionName: "First Division",
                    CompetitionStartYear: 2000,
                    CompetitionEndYear: 2001,
                    CompetitionTier: 1,
                    CompetitionRegion: null,
                    RulesType: "League",
                    RulesStage: null,
                    RulesExtraTime: false,
                    RulesPenalties: false,
                    RulesNumLegs: null,
                    RulesAwayGoals: false,
                    RulesReplays: false,
                    HomeTeamId: 1,
                    HomeTeamName: "Accrington Stanley",
                    HomeTeamAbbreviation: "ACC",
                    AwayTeamId: 2,
                    AwayTeamName: "AFC Bournemouth",
                    AwayTeamAbbreviation: "BOU",
                    HomeGoals: 1,
                    AwayGoals: 0,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0)
            };
            
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatches(null, null, null, null, null))
                .Returns(models);

            var builder = new MatchBuilder(mockMatchRepository.Object);

            var matches = builder.BuildMatches(null, null, null, null, null);

            Assert.That(matches.Count, Is.EqualTo(1));

            var actualMatch = matches.Single();
            var expectedMatchModel = models.Single();
            
            Assert.That(actualMatch.Id, Is.EqualTo(expectedMatchModel.Id));
            Assert.That(actualMatch.HomeTeam.Name, Is.EqualTo(expectedMatchModel.HomeTeamName));
            Assert.That(actualMatch.Rules.Type, Is.EqualTo(expectedMatchModel.RulesType));
            Assert.That(actualMatch.Competition.Level, Is.EqualTo(expectedMatchModel.CompetitionLevel));
        }
        
        [Test]
        public void BuildMatch_returns_null_given_null_model()
        {
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatch(It.IsAny<long>()))
                .Returns((MatchModel?) null);

            var builder = new MatchBuilder(mockMatchRepository.Object);

            var match = builder.BuildMatch(1);

            Assert.That(match, Is.Null);
        }
        
        [Test]
        public void BuildMatch_does_not_handle_repository_exception()
        {
            var exception = new InvalidOperationException("An error occurred");

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatch(It.IsAny<long>()))
                .Throws(exception);

            var builder = new MatchBuilder(mockMatchRepository.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => builder.BuildMatch(1));

            Assert.That(ex, Is.EqualTo(exception));
        }

        [Test]
        public void BuildMatch_returns_domain_object_given_model()
        {
            var expectedMatchModel = new MatchModel(
                Id: 1,
                MatchDate: new DateTime(2000, 1, 1),
                CompetitionId: 1,
                CompetitionName: "First Division",
                CompetitionStartYear: 2000,
                CompetitionEndYear: 2001,
                CompetitionTier: 1,
                CompetitionRegion: null,
                RulesType: "League",
                RulesStage: null,
                RulesExtraTime: false,
                RulesPenalties: false,
                RulesNumLegs: null,
                RulesAwayGoals: false,
                RulesReplays: false,
                HomeTeamId: 1,
                HomeTeamName: "Accrington Stanley",
                HomeTeamAbbreviation: "ACC",
                AwayTeamId: 2,
                AwayTeamName: "AFC Bournemouth",
                AwayTeamAbbreviation: "BOU",
                HomeGoals: 1,
                AwayGoals: 0,
                HomeGoalsExtraTime: 0,
                AwayGoalsExtraTime: 0,
                HomePenaltiesTaken: 0,
                HomePenaltiesScored: 0,
                AwayPenaltiesTaken: 0,
                AwayPenaltiesScored: 0);
            
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetMatch(1))
                .Returns(expectedMatchModel);

            var builder = new MatchBuilder(mockMatchRepository.Object);

            var actualMatch = builder.BuildMatch(1);

            Assert.That(actualMatch, Is.Not.Null);

            Assert.That(actualMatch!.Id, Is.EqualTo(expectedMatchModel.Id));
            Assert.That(actualMatch.HomeTeam.Name, Is.EqualTo(expectedMatchModel.HomeTeamName));
            Assert.That(actualMatch.Rules.Type, Is.EqualTo(expectedMatchModel.RulesType));
            Assert.That(actualMatch.Competition.Level, Is.EqualTo(expectedMatchModel.CompetitionLevel));
        }
    }
}