using System.Net;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class TeamControllerTests
{
    [Test]
    public async Task GetAllTeams_returns_not_found_given_no_matching_teams()
    {
        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildAllTeams())
            .Returns(Array.Empty<Team>());

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No teams were found."));
    }
        
    [Test]
    public async Task GetAllTeams_returns_internal_server_error_given_exception()
    {
        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildAllTeams())
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetAllTeams_returns_teams()
    {
        var expectedTeams = new []
        {
            new Team(Id: 1, Name: "Norwich City", "NOR", null)
        };

        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildAllTeams())
            .Returns(expectedTeams);

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualTeams = JsonConvert.DeserializeObject<Team[]>(responseString);
            
        Assert.That(actualTeams, Is.EqualTo(expectedTeams));
    }
        
    [Test]
    public async Task GetTeam_returns_not_found_given_no_matching_team()
    {
        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildTeam(It.IsAny<long>()))
            .Returns((Team?) null);

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No team was found with id 1."));
    }
        
    [Test]
    public async Task GetTeam_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();

        var response = await client.GetAsync("api/v2/teams/not-an-id");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetTeam_returns_internal_server_error_given_exception()
    {
        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildTeam(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetTeam_returns_team()
    {
        var expectedTeam = new Team(Id: 1, Name: "Norwich City", "NOR", null);

        var mockTeamBuilder = new Mock<ITeamBuilder>();
        mockTeamBuilder
            .Setup(x => x.BuildTeam(1))
            .Returns(expectedTeam);

        var client = GetTestClient(mockTeamBuilder);

        var response = await client.GetAsync("api/v2/teams/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualTeam = JsonConvert.DeserializeObject<Team>(responseString);
            
        Assert.That(actualTeam, Is.EqualTo(expectedTeam));
    }
        
    private static HttpClient GetTestClient(IMock<ITeamBuilder> mockTeamBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s =>
            {
                s.SwapTransient(mockTeamBuilder.Object);
            });
        });
            
        return factory.CreateClient();
    }
        
    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}