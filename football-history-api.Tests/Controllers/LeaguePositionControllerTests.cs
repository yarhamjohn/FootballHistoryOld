using System.Net;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class LeaguePositionControllerTests
{
    [Test]
    public async Task GetLeaguePositions_returns_not_found_given_no_available_positions()
    {
        var mockLeaguePositionBuilder = new Mock<ILeaguePositionBuilder>();
        mockLeaguePositionBuilder
            .Setup(x => x.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(Array.Empty<LeaguePosition>());

        var client = GetTestClient(mockLeaguePositionBuilder);

        var response = await client.GetAsync("api/v2/league-positions/competition/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No league positions found for teamId 2 and competitionId 1."));
    }

    [Test]
    public async Task GetLeaguePositions_returns_internal_server_error_given_exception()
    {
        var mockLeaguePositionBuilder = new Mock<ILeaguePositionBuilder>();
        mockLeaguePositionBuilder
            .Setup(x => x.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockLeaguePositionBuilder);

        var response = await client.GetAsync("api/v2/league-positions/competition/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [TestCase("league-positions")]
    [TestCase("league-positions/competition")]
    [TestCase("league-positions/team")]
    [TestCase("league-positions/competition/1")]
    [TestCase("league-positions/team/2")]
    [TestCase("league-positions/competition/1/team")]
    [TestCase("league-positions/team/2/competition")]
    [TestCase("league-positions/competition/not-an-id/team/2")]
    [TestCase("league-positions/competition/1/team/not-an-id")]
    [TestCase("league-positions/team/2/competition/1")]
    public async Task GetLeaguePositions_returns_not_found_given_invalid_id(string url)
    {
        var client = GetTestClient();

        var response = await client.GetAsync($"api/v2/{url}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetLeaguePositions_returns_league_positions()
    {
        var expectedLeaguePositions = new[]
        {
            new LeaguePosition(Date: new DateTime(2000, 1, 1), Position: 1)
        };

        var mockLeaguePositionBuilder = new Mock<ILeaguePositionBuilder>();
        mockLeaguePositionBuilder
            .Setup(x => x.GetPositions(1, 2))
            .Returns(expectedLeaguePositions);

        var client = GetTestClient(mockLeaguePositionBuilder);

        var response = await client.GetAsync("api/v2/league-positions/competition/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualLeaguePositions = JsonConvert.DeserializeObject<LeaguePosition[]>(responseString);

        Assert.That(actualLeaguePositions.Length, Is.EqualTo(1));
        Assert.That(actualLeaguePositions.Single(), Is.EqualTo(expectedLeaguePositions.Single()));
    }

    private static HttpClient GetTestClient(IMock<ILeaguePositionBuilder> mockLeaguePositionBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s => { s.SwapTransient(mockLeaguePositionBuilder.Object); });
        });

        return factory.CreateClient();
    }

    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}