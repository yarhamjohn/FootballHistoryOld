using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers;

[TestFixture]
public class StatisticsControllerTests
{
    [Test]
    public async Task GetSeasonStatistics_returns_not_found_given_no_available_statistics()
    {
        var mockStatisticsBuilder = new Mock<IStatisticsBuilder>();
        mockStatisticsBuilder
            .Setup(x => x.BuildSeasonStatistics(It.IsAny<long>()))
            .Returns(Array.Empty<CategorisedStatistics>());

        var client = GetTestClient(mockStatisticsBuilder);

        var response = await client.GetAsync("api/v2/statistics/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Is.EqualTo("No statistics available for the specified season (1)."));
    }

    [Test]
    public async Task GetSeasonStatistics_returns_internal_server_error_given_exception()
    {
        var mockStatisticsBuilder = new Mock<IStatisticsBuilder>();
        mockStatisticsBuilder
            .Setup(x => x.BuildSeasonStatistics(It.IsAny<long>()))
            .Throws(new Exception("error message"));

        var client = GetTestClient(mockStatisticsBuilder);

        var response = await client.GetAsync("api/v2/statistics/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("error message"));
    }

    [TestCase(null)]
    [TestCase("not-an-id")]
    public async Task GetSeasonStatistics_returns_not_found_given_invalid_id(string? id)
    {
        var client = GetTestClient();

        var response = await client.GetAsync($"api/v2/statistics/season/{id}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetSeasonStatistics_returns_statistics()
    {
        var expectedStatistics = new[]
        {
            new CategorisedStatistics(
                Category: "Some-Category",
                Statistics: new Statistic[]
                {
                    new(Name: "Stat-1",
                        Value: 2.0,
                        TeamName: "Norwich City",
                        CompetitionName: "First Division")
                })
        };

        var mockStatisticsBuilder = new Mock<IStatisticsBuilder>();
        mockStatisticsBuilder
            .Setup(x => x.BuildSeasonStatistics(1))
            .Returns(expectedStatistics);

        var client = GetTestClient(mockStatisticsBuilder);

        var response = await client.GetAsync("api/v2/statistics/season/1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualStatistics = JsonConvert.DeserializeObject<CategorisedStatistics[]>(responseString);

        Assert.That(actualStatistics.Length, Is.EqualTo(1));

        var actual = actualStatistics.Single();
        var expected = expectedStatistics.Single();

        Assert.That(actual.Category, Is.EqualTo(expected.Category));
        Assert.That(actual.Statistics, Is.EquivalentTo(expected.Statistics));
    }

    private static HttpClient GetTestClient(IMock<IStatisticsBuilder> mockStatisticsBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s => { s.SwapTransient(mockStatisticsBuilder.Object); });
        });

        return factory.CreateClient();
    }

    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}