using System.Net;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class LeagueTableControllerTests
{
    [Test]
    public async Task GetLeagueTable_from_competition_id_returns_not_found_given_no_league_table()
    {
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(It.IsAny<long>()))
            .Returns((LeagueTable?)null);

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/competition/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No league table available for competition 1."));
    }

    [Test]
    public async Task GetLeagueTable_from_competition_id_returns_internal_server_error_given_exception()
    {
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/competition/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [Test]
    public async Task GetLeagueTable_from_competition_id_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();

        var response = await client.GetAsync("api/v2/league-table/competition/not-an-id");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetLeagueTable_from_competition_id_returns_league_table()
    {
        var expectedLeagueTable = new LeagueTable(
            Table: new[] { new LeagueTableRow { TeamId = 1, Position = 1, Points = 3 } },
            Competition: new Competition(Id: 1,
                Name: "First Division",
                Season: new(Id: 1, StartYear: 2000, EndYear: 2001),
                Level: "1",
                Comment: null,
                Rules: new(
                    PointsForWin: 1,
                    TotalPlaces: 1,
                    PromotionPlaces: 1,
                    RelegationPlaces: 1,
                    PlayOffPlaces: 1,
                    RelegationPlayOffPlaces: 1,
                    ReElectionPlaces: 1,
                    FailedReElectionPosition: null))
        );

        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(1))
            .Returns(expectedLeagueTable);

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/competition/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualLeagueTable = JsonConvert.DeserializeObject<LeagueTable>(responseString);

        var actualTableRow = actualLeagueTable.Table.Single();
        var expectedTableRow = expectedLeagueTable.Table.Single();
        
        Assert.That(actualTableRow.TeamId, Is.EqualTo(expectedTableRow.TeamId));
        Assert.That(actualTableRow.Position, Is.EqualTo(expectedTableRow.Position));
        Assert.That(actualTableRow.Points, Is.EqualTo(expectedTableRow.Points));
        Assert.That(actualLeagueTable.Competition, Is.EqualTo(expectedLeagueTable.Competition));
    }

    [Test]
    public async Task GetLeagueTable_from_season_id_and_team_id_returns_not_found_given_no_league_table()
    {
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(It.IsAny<long>(), It.IsAny<long>()))
            .Returns((LeagueTable?)null);

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/season/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No league table available for season 1 and team 2."));
    }

    [Test]
    public async Task GetLeagueTable_from_season_id_and_team_id_returns_internal_server_error_given_exception()
    {
        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(It.IsAny<long>(), It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/season/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [TestCase("league-table")]
    [TestCase("league-table/season")]
    [TestCase("league-table/team")]
    [TestCase("league-table/season/1")]
    [TestCase("league-table/team/2")]
    [TestCase("league-table/season/1/team")]
    [TestCase("league-table/team/2/season")]
    [TestCase("league-table/season/not-an-id/team/2")]
    [TestCase("league-table/season/1/team/not-an-id")]
    [TestCase("league-table/team/2/season/1")]
    public async Task GetLeagueTable_from_competition_id_returns_not_found_given_invalid_id(string url)
    {
        var client = GetTestClient();

        var response = await client.GetAsync($"api/v2/{url}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetLeagueTable_from_season_id_and_team_id_returns_league_table()
    {
        var expectedLeagueTable = new LeagueTable(
            Table: new[] { new LeagueTableRow { TeamId = 1, Position = 1, Points = 3 } },
            Competition: new Competition(Id: 1,
                Name: "First Division",
                Season: new(Id: 1, StartYear: 2000, EndYear: 2001),
                Level: "1",
                Comment: null,
                Rules: new(
                    PointsForWin: 1,
                    TotalPlaces: 1,
                    PromotionPlaces: 1,
                    RelegationPlaces: 1,
                    PlayOffPlaces: 1,
                    RelegationPlayOffPlaces: 1,
                    ReElectionPlaces: 1,
                    FailedReElectionPosition: null))
        );

        var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
        mockLeagueTableBuilder
            .Setup(x => x.BuildFullLeagueTable(1, 2))
            .Returns(expectedLeagueTable);

        var client = GetTestClient(mockLeagueTableBuilder);

        var response = await client.GetAsync("api/v2/league-table/season/1/team/2");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualLeagueTable = JsonConvert.DeserializeObject<LeagueTable>(responseString);

        var actualTableRow = actualLeagueTable.Table.Single();
        var expectedTableRow = expectedLeagueTable.Table.Single();
        
        Assert.That(actualTableRow.TeamId, Is.EqualTo(expectedTableRow.TeamId));
        Assert.That(actualTableRow.Position, Is.EqualTo(expectedTableRow.Position));
        Assert.That(actualTableRow.Points, Is.EqualTo(expectedTableRow.Points));
        Assert.That(actualLeagueTable.Competition, Is.EqualTo(expectedLeagueTable.Competition));
    }

    private static HttpClient GetTestClient(IMock<ILeagueTableBuilder> mockLeagueTableBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s => { s.SwapTransient(mockLeagueTableBuilder.Object); });
        });

        return factory.CreateClient();
    }

    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}