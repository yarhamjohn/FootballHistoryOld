using System;
using System.Linq;
using football.history.api.Models;
using football.history.api.Repositories;
using football.history.api.Tests.TestUtilities;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories;

[TestFixture]
public class MatchRepositoryTests
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
    public void GetMatches_returns_all_matches_given_no_parameters()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatches();

        Assert.That(result.Length, Is.EqualTo(2));
    }

    [TestCase(0, 1, 1, MatchType.League, "2000-01-01")]
    [TestCase(1, 0, 1, MatchType.League, "2000-01-01")]
    [TestCase(1, 1, 0, MatchType.League, "2000-01-01")]
    [TestCase(2, 1, 1, MatchType.PlayOff, "2000-01-02")]
    [TestCase(1, 1, 1, MatchType.League, "2000-01-02")]
    public void GetMatches_returns_empty_array_given_a_non_matching_parameter(
        long? competitionId,
        long? seasonId,
        long? teamId,
        MatchType? type,
        DateTime? matchDate)
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatches(competitionId, seasonId, teamId, type, matchDate);

        Assert.That(result, Is.Empty);
    }

    [TestCase(1, 1, null, MatchType.League, "2000-01-01")]
    [TestCase(null, 1, 1, MatchType.PlayOff, "2000-01-02")]
    public void GetMatches_returns_correct_matches_given_matching_or_missing_parameters(
        long? competitionId,
        long? seasonId,
        long? teamId,
        MatchType? type,
        DateTime? matchDate)
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatches(competitionId, seasonId, teamId, type, matchDate);

        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result.Single().RulesType, Is.EqualTo(type.ToString()));
        Assert.That(result.Single().MatchDate, Is.EqualTo(matchDate));
    }

    [TestCase(1, 1, 1, MatchType.League, "2000-01-01")]
    [TestCase(1, 1, 1, MatchType.PlayOff, "2000-01-02")]
    public void GetMatches_returns_correct_matches_given_complete_matching_parameters(
        long? competitionId,
        long? seasonId,
        long? teamId,
        MatchType? type,
        DateTime? matchDate)
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatches(competitionId, seasonId, teamId, type, matchDate);

        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result.Single().RulesType, Is.EqualTo(type.ToString()));
        Assert.That(result.Single().MatchDate, Is.EqualTo(matchDate));
    }

    [Test]
    public void GetLeagueMatches_returns_empty_array_given_no_matching_competitionId()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetLeagueMatches(0);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetLeagueMatches_returns_empty_array_given_matching_competitionId_but_no_league_matches()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetLeagueMatches(2);

        Assert.That(result, Is.Empty);
    }
        
    [Test]
    public void GetLeagueMatches_returns_match_models_given_matching_league_matches()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetLeagueMatches(1);

        var expectedModel = new[]
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

        Assert.That(result, Is.EqualTo(expectedModel));
    }

    [Test]
    public void GetPlayOffMatches_returns_empty_array_given_no_matching_competitionId()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetLeagueMatches(2);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPlayOffMatches_returns_empty_array_given_matching_competitionId_but_no_playoff_matches()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetLeagueMatches(2);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPlayOffMatches_returns_match_models_given_matching_playoff_matches()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetPlayOffMatches(1);

        var expectedModel = new[]
        {
            new MatchModel(
                Id: 2,
                MatchDate: new DateTime(2000, 1, 2),
                CompetitionId: 1,
                CompetitionName: "First Division",
                CompetitionStartYear: 2000,
                CompetitionEndYear: 2001,
                CompetitionTier: 1,
                CompetitionRegion: null,
                RulesType: "PlayOff",
                RulesStage: "Final",
                RulesExtraTime: true,
                RulesPenalties: true,
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

        Assert.That(result, Is.EqualTo(expectedModel));
    }

    [Test]
    public void GetMatch_returns_null_given_no_matching_matchId()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatch(0);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetMatch_returns_match_model_given_matching_matchId()
    {
        var repository = new MatchRepository(_testDbConnection);

        var result = repository.GetMatch(1);

        var expectedModel = new MatchModel(
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

        Assert.That(result, Is.EqualTo(expectedModel));
    }
}