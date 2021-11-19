using System.Net;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class CompetitionControllerTests
{
    [Test]
    public async Task GetAllCompetitions_returns_not_found_given_no_matching_competitions()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(null))
            .Returns(Array.Empty<Competition>());

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No competitions were found."));
    }
        
    [Test]
    public async Task GetAllCompetitions_returns_internal_server_error_given_exception()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(null))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetAllCompetitions_returns_competitions()
    {
        var expectedCompetitions = new []
        {
            new Competition(Id: 1,
                Name: "First Division",
                Season: new (Id: 1, StartYear: 2000, EndYear: 2001),
                Level: "1",
                Comment: null,
                Rules: new (
                    PointsForWin: 1,
                    TotalPlaces: 1,
                    PromotionPlaces: 1,
                    RelegationPlaces: 1,
                    PlayOffPlaces: 1,
                    RelegationPlayOffPlaces: 1,
                    ReElectionPlaces: 1,
                    FailedReElectionPosition: null))
        };

        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(null))
            .Returns(expectedCompetitions);

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualCompetitions = JsonConvert.DeserializeObject<Competition[]>(responseString);
            
        Assert.That(actualCompetitions, Is.EqualTo(expectedCompetitions));
    }
        
    [Test]
    public async Task GetCompetitions_returns_not_found_given_no_matching_competitions()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(It.IsAny<long>()))
            .Returns(Array.Empty<Competition>());

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No competitions were found."));
    }
        
    [TestCase(null)]
    [TestCase("not-an-id")]
    public async Task GetCompetitions_returns_not_found_given_invalid_id(string? id)
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        
        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync($"api/v2/competitions/season/{id}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
        
    [Test]
    public async Task GetCompetitions_returns_internal_server_error_given_exception()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetCompetitions_returns_competitions()
    {
        var expectedCompetitions = new []
        {
            new Competition(Id: 1,
                Name: "First Division",
                Season: new (Id: 1, StartYear: 2000, EndYear: 2001),
                Level: "1",
                Comment: null,
                Rules: new (
                    PointsForWin: 1,
                    TotalPlaces: 1,
                    PromotionPlaces: 1,
                    RelegationPlaces: 1,
                    PlayOffPlaces: 1,
                    RelegationPlayOffPlaces: 1,
                    ReElectionPlaces: 1,
                    FailedReElectionPosition: null))
        };

        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetitions(It.IsAny<long>()))
            .Returns(expectedCompetitions);

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualCompetitions = JsonConvert.DeserializeObject<Competition[]>(responseString);
            
        Assert.That(actualCompetitions, Is.EqualTo(expectedCompetitions));
    }
        
    [Test]
    public async Task GetCompetition_returns_not_found_given_no_matching_competition()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetition(It.IsAny<long>()))
            .Returns((Competition?) null);

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No competition was found with id 1."));
    }
        
    [Test]
    public async Task GetCompetition_returns_not_found_given_invalid_id()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        
        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/not-an-id");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
        
    [Test]
    public async Task GetCompetition_returns_internal_server_error_given_exception()
    {
        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetition(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetCompetition_returns_competition()
    {
        var expectedCompetition = 
            new Competition(Id: 1,
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
                    FailedReElectionPosition: null));

        var mockCompetitionBuilder = new Mock<ICompetitionBuilder>();
        mockCompetitionBuilder
            .Setup(x => x.BuildCompetition(It.IsAny<long>()))
            .Returns(expectedCompetition);

        var client = GetTestClient(mockCompetitionBuilder);

        var response = await client.GetAsync("api/v2/competitions/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualCompetition = JsonConvert.DeserializeObject<Competition>(responseString);
            
        Assert.That(actualCompetition, Is.EqualTo(expectedCompetition));
    }
        
    private static HttpClient GetTestClient(IMock<ICompetitionBuilder> mockCompetitionBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s =>
            {
                s.SwapTransient(mockCompetitionBuilder.Object);
            });
        });
            
        return factory.CreateClient();
    }
}