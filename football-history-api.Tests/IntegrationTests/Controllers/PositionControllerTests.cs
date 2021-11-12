using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.IntegrationTests.Controllers;

[TestFixture]
public class PositionControllerTests
{
    [Test]
    public async Task GetSeasonPositions_returns_not_found_given_no_matching_season()
    {
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildSeasonPositions(It.IsAny<long>()))
            .Returns(Array.Empty<Position>());

        var client = GetTestClient(mockPositionBuilder);

        var response = await client.GetAsync("api/v2/positions/season/0");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No positions were found for the given season (0)."));
    }
    
    [Test]
    public async Task GetSeasonPositions_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();
    
        var response = await client.GetAsync("api/v2/positions/season/not-an-id");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetSeasonPositions_returns_internal_server_error_given_exception()
    {
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildSeasonPositions(It.IsAny<long>()))
            .Throws(new Exception("error message"));
    
        var client = GetTestClient(mockPositionBuilder);
    
        var response = await client.GetAsync("api/v2/positions/season/0");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetSeasonPositions_returns_positions()
    {
        var expectedPositions = new []
        {
            new Position(
                Id: 1,
                CompetitionId: 1,
                CompetitionName: "Premier League",
                TeamId: 1,
                TeamName: "Norwich City",
                LeaguePosition: 1,
                Status: "Champions")
        };
    
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildSeasonPositions(1))
            .Returns(expectedPositions);
    
        var client = GetTestClient(mockPositionBuilder);
    
        var response = await client.GetAsync("api/v2/positions/season/1");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    
        var responseString = await response.Content.ReadAsStringAsync();
        var actualPositions = JsonConvert.DeserializeObject<Position[]>(responseString);
            
        Assert.That(actualPositions, Is.EqualTo(expectedPositions));
    }
    
    [Test]
    public async Task GetTeamPositions_returns_not_found_given_no_matching_team()
    {
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildTeamPositions(It.IsAny<long>()))
            .Returns(Array.Empty<Position>());

        var client = GetTestClient(mockPositionBuilder);

        var response = await client.GetAsync("api/v2/positions/team/0");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No positions were found for the given team (0)."));
    }
    
    [Test]
    public async Task GetTeamPositions_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();
    
        var response = await client.GetAsync("api/v2/positions/team/not-an-id");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetTeamPositions_returns_internal_server_error_given_exception()
    {
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildTeamPositions(It.IsAny<long>()))
            .Throws(new Exception("error message"));
    
        var client = GetTestClient(mockPositionBuilder);
    
        var response = await client.GetAsync("api/v2/positions/team/0");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetTeamPositions_returns_positions()
    {
        var expectedPositions = new []
        {
            new Position(
                Id: 1,
                CompetitionId: 1,
                CompetitionName: "Premier League",
                TeamId: 1,
                TeamName: "Norwich City",
                LeaguePosition: 1,
                Status: "Champions")
        };
    
        var mockPositionBuilder = new Mock<IPositionBuilder>();
        mockPositionBuilder
            .Setup(x => x.BuildTeamPositions(1))
            .Returns(expectedPositions);
    
        var client = GetTestClient(mockPositionBuilder);
    
        var response = await client.GetAsync("api/v2/positions/team/1");
    
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    
        var responseString = await response.Content.ReadAsStringAsync();
        var actualPositions = JsonConvert.DeserializeObject<Position[]>(responseString);
            
        Assert.That(actualPositions, Is.EqualTo(expectedPositions));
    }

    private static HttpClient GetTestClient(IMock<IPositionBuilder> mockPositionBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s =>
            {
                s.SwapTransient(mockPositionBuilder.Object);
            });
        });
            
        return factory.CreateClient();
    }
        
    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}