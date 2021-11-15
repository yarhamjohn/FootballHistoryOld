using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using football.history.api.Builders;
using football.history.api.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Match = football.history.api.Domain.Match;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class MatchControllerTests
{
    [Test]
    public async Task GetAllMatches_returns_not_found_given_no_matching_matches()
    {
        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatches(null, null, null, null, null))
            .Returns(Array.Empty<Match>());

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No matches were found."));
    }

    [Test]
    public async Task GetAllMatches_returns_internal_server_error_given_exception()
    {
        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatches(null, null, null, null, null))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [Test]
    public async Task GetAllMatches_returns_matches()
    {
        var expectedMatches = new[]
        {
            new Match(
                Id: 1,
                MatchDate: new DateTime(2000, 1, 1),
                Competition: new(
                    Id: 1,
                    Name: "First Division",
                    StartYear: 2000,
                    EndYear: 2001,
                    Level: "1"),
                Rules: new(
                    Type: "League",
                    Stage: null,
                    ExtraTime: false,
                    Penalties: false,
                    NumLegs: null,
                    AwayGoals: false,
                    Replays: false),
                HomeTeam: new(
                    Id: 1,
                    Name: "Accrington Stanley",
                    Abbreviation: "ACC",
                    Goals: 1,
                    GoalsExtraTime: 0,
                    PenaltiesTaken: 0,
                    PenaltiesScored: 0),
                AwayTeam: new(
                    Id: 2,
                    Name: "AFC Bournemouth",
                    Abbreviation: "BOU",
                    Goals: 0,
                    GoalsExtraTime: 0,
                    PenaltiesTaken: 0,
                    PenaltiesScored: 0)
            )
        };

        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatches(null, null, null, null, null))
            .Returns(expectedMatches);

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualMatchs = JsonConvert.DeserializeObject<Match[]>(responseString);

        Assert.That(actualMatchs, Is.EqualTo(expectedMatches));
    }

    [TestCase(1, 2, 3, MatchType.League, "2000-01-01", 
        "api/v2/matches?competitionId=1&seasonId=2&teamId=3&type=League&matchDate=2000-01-01")]
    [TestCase(null, 2, 3, MatchType.League, "2000-01-01", 
        "api/v2/matches?seasonId=2&teamId=3&type=League&matchDate=2000-01-01")]
    [TestCase(1, null, 3, MatchType.League, "2000-01-01", 
        "api/v2/matches?competitionId=1&teamId=3&type=League&matchDate=2000-01-01")]
    [TestCase(1, 2, null, MatchType.League, "2000-01-01", 
        "api/v2/matches?competitionId=1&seasonId=2&type=League&matchDate=2000-01-01")]
    [TestCase(1, 2, 3, null, "2000-01-01", 
        "api/v2/matches?competitionId=1&seasonId=2&teamId=3&matchDate=2000-01-01")]
    [TestCase(1, 2, 3, MatchType.League, null, 
        "api/v2/matches?competitionId=1&seasonId=2&teamId=3&type=League")]
    [TestCase(null, null, null, null, null, 
        "api/v2/matches")]
    public async Task GetAllMatches_passes_all_provided_parameters_to_builder(
        long? competitionId, long? seasonId, long? teamId, MatchType? type, DateTime? matchDate, string url)
    {
        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatches(competitionId, seasonId, teamId, type, matchDate))
            .Returns(Array.Empty<Match>());

        var client = GetTestClient(mockMatchBuilder);

        await client.GetAsync(url);

        mockMatchBuilder.VerifyAll();
    }

    [Test]
    public async Task GetMatch_returns_not_found_given_no_matching_match()
    {
        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatch(It.IsAny<long>()))
            .Returns((Match?)null);

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No match was found with id 1."));
    }

    [Test]
    public async Task GetMatch_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();

        var response = await client.GetAsync("api/v2/matches/not-an-id");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetMatch_returns_internal_server_error_given_exception()
    {
        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatch(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [Test]
    public async Task GetMatch_returns_match()
    {
        var expectedMatch = new Match(
            Id: 1,
            MatchDate: new DateTime(2000, 1, 1),
            Competition: new(
                Id: 1,
                Name: "First Division",
                StartYear: 2000,
                EndYear: 2001,
                Level: "1"),
            Rules: new(
                Type: "League",
                Stage: null,
                ExtraTime: false,
                Penalties: false,
                NumLegs: null,
                AwayGoals: false,
                Replays: false),
            HomeTeam: new(
                Id: 1,
                Name: "Accrington Stanley",
                Abbreviation: "ACC",
                Goals: 1,
                GoalsExtraTime: 0,
                PenaltiesTaken: 0,
                PenaltiesScored: 0),
            AwayTeam: new(
                Id: 2,
                Name: "AFC Bournemouth",
                Abbreviation: "BOU",
                Goals: 0,
                GoalsExtraTime: 0,
                PenaltiesTaken: 0,
                PenaltiesScored: 0)
        );

        var mockMatchBuilder = new Mock<IMatchBuilder>();
        mockMatchBuilder
            .Setup(x => x.BuildMatch(1))
            .Returns(expectedMatch);

        var client = GetTestClient(mockMatchBuilder);

        var response = await client.GetAsync("api/v2/matches/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualMatch = JsonConvert.DeserializeObject<Match>(responseString);

        Assert.That(actualMatch, Is.EqualTo(expectedMatch));
    }

    private static HttpClient GetTestClient(IMock<IMatchBuilder> mockMatchBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s => { s.SwapTransient(mockMatchBuilder.Object); });
        });

        return factory.CreateClient();
    }

    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}