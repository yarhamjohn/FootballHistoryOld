using System.Net;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class SeasonControllerTests
{
    [Test]
    public async Task GetAllSeasons_returns_not_found_given_no_matching_seasons()
    {
        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildAllSeasons())
            .Returns(Array.Empty<Season>());

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No seasons were found."));
    }
        
    [Test]
    public async Task GetAllSeasons_returns_internal_server_error_given_exception()
    {
        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildAllSeasons())
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetAllSeasons_returns_seasons()
    {
        var expectedSeasons = new []
        {
            new Season(Id: 1, StartYear: 2000, EndYear: 2001)
        };

        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildAllSeasons())
            .Returns(expectedSeasons);

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualSeasons = JsonConvert.DeserializeObject<Season[]>(responseString);
            
        Assert.That(actualSeasons, Is.EqualTo(expectedSeasons));
    }
        
    [Test]
    public async Task GetSeason_returns_not_found_given_no_matching_season()
    {
        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildSeason(It.IsAny<long>()))
            .Returns((Season?) null);

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No season was found with id 1."));
    }
        
    [Test]
    public async Task GetSeason_returns_not_found_given_invalid_id()
    {
        var client = GetTestClient();

        var response = await client.GetAsync("api/v2/seasons/not-an-id");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetSeason_returns_internal_server_error_given_exception()
    {
        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildSeason(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }
        
    [Test]
    public async Task GetSeason_returns_season()
    {
        var expectedSeason = new Season(Id: 1, StartYear: 2000, EndYear: 2001);

        var mockSeasonBuilder = new Mock<ISeasonBuilder>();
        mockSeasonBuilder
            .Setup(x => x.BuildSeason(1))
            .Returns(expectedSeason);

        var client = GetTestClient(mockSeasonBuilder);

        var response = await client.GetAsync("api/v2/seasons/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualSeason = JsonConvert.DeserializeObject<Season>(responseString);
            
        Assert.That(actualSeason, Is.EqualTo(expectedSeason));
    }
        
    private static HttpClient GetTestClient(IMock<ISeasonBuilder> mockSeasonBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s =>
            {
                s.SwapTransient(mockSeasonBuilder.Object);
            });
        });
            
        return factory.CreateClient();
    }
        
    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}