namespace football.history.api.Tests.Controllers;

[TestFixture]
public class HistoricalPositionControllerTests
{
    [Test]
    public async Task GetHistoricalRecord_returns_not_found_given_invalid_url()
    {
        var client = GetTestClient();

        var response = await client.GetAsync("api/v2/historical-record");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [TestCase("api/v2/historical-record/teamId/1")]
    [TestCase("api/v2/historical-record/teamId/1?seasonIds=")]
    public async Task GetHistoricalRecord_calls_builder_given_no_seasonIds(string url)
    {
        var mockHistoricalRecordBuilder = new Mock<IHistoricalRecordBuilder>();
        mockHistoricalRecordBuilder
            .Setup(x => x.Build(1, Array.Empty<long>()))
            .Returns(new HistoricalRecord(1, Array.Empty<HistoricalSeason>()));
            
        var client = GetTestClient(mockHistoricalRecordBuilder);

        await client.GetAsync(url);
            
        mockHistoricalRecordBuilder.VerifyAll();
    }

    [Test]
    public async Task GetHistoricalRecord_returns_internal_server_error_given_data_invalid_exception()
    {
        var mockHistoricalRecordBuilder = new Mock<IHistoricalRecordBuilder>();
        mockHistoricalRecordBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<long[]>()))
            .Throws(new DataInvalidException("Data was invalid."));

        var client = GetTestClient(mockHistoricalRecordBuilder);

        var response = await client.GetAsync("api/v2/historical-record/teamId/1?seasonIds=1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("Invalid data was found."));
        Assert.That(responseString, Does.Contain("Data was invalid."));
    }
        
    [Test]
    public async Task GetHistoricalRecord_returns_internal_server_error_given_unhandled_exception()
    {
        var mockHistoricalRecordBuilder = new Mock<IHistoricalRecordBuilder>();
        mockHistoricalRecordBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<long[]>()))
            .Throws(new Exception("Something went wrong."));

        var client = GetTestClient(mockHistoricalRecordBuilder);

        var response = await client.GetAsync("api/v2/historical-record/teamId/1?seasonIds=1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("Something went wrong."));
    }
        
    [Test]
    public async Task GetHistoricalRecord_returns_not_found_given_no_historical_seasons()
    {
        var mockHistoricalRecordBuilder = new Mock<IHistoricalRecordBuilder>();
        mockHistoricalRecordBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<long[]>()))
            .Returns(new HistoricalRecord(1, Array.Empty<HistoricalSeason>()));

        var client = GetTestClient(mockHistoricalRecordBuilder);

        var response = await client.GetAsync("api/v2/historical-record/teamId/1?seasonIds=1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.That(responseString, Does.Contain("No historical seasons were found for the specified team"));
        Assert.That(responseString, Does.Contain("('1') and seasonIds ('1')"));
    }

    [Test]
    public async Task GetHistoricalRecord_returns_record()
    {
        var expectedHistoricalRecord = new HistoricalRecord(1,
            new [] { new HistoricalSeason(1, 2000)});
            
        var mockHistoricalRecordBuilder = new Mock<IHistoricalRecordBuilder>();
        mockHistoricalRecordBuilder
            .Setup(x => x.Build(1, new [] { 1L }))
            .Returns(expectedHistoricalRecord);

        var client = GetTestClient(mockHistoricalRecordBuilder);

        var response = await client.GetAsync("api/v2/historical-record/teamId/1?seasonIds=1");
            
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseString = await response.Content.ReadAsStringAsync();
        var actualHistoricalRecord = JsonConvert.DeserializeObject<HistoricalRecord>(responseString);

        Assert.That(actualHistoricalRecord.TeamId, Is.EqualTo(expectedHistoricalRecord.TeamId));
        Assert.That(actualHistoricalRecord.HistoricalSeasons.Length, Is.EqualTo(1));
            
        var actualHistoricalSeason = actualHistoricalRecord.HistoricalSeasons.Single();
        var expectedHistoricalSeason = expectedHistoricalRecord.HistoricalSeasons.Single();
            
        Assert.That(actualHistoricalSeason.SeasonId, Is.EqualTo(expectedHistoricalSeason.SeasonId));
        Assert.That(actualHistoricalSeason.SeasonStartYear, Is.EqualTo(expectedHistoricalSeason.SeasonStartYear));
        Assert.That(actualHistoricalSeason.Boundaries, Is.Empty);
        Assert.That(actualHistoricalSeason.HistoricalPosition,
            Is.EqualTo(expectedHistoricalSeason.HistoricalPosition));
    }

    private static HttpClient GetTestClient(IMock<IHistoricalRecordBuilder> mockHistoricalRecordBuilder)
    {
        var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(b =>
        {
            b.ConfigureServices(s =>
            {
                s.SwapTransient(mockHistoricalRecordBuilder.Object);
            });
        });
            
        return factory.CreateClient();
    }
        
    private static HttpClient GetTestClient() => new WebApplicationFactory<Startup>().CreateClient();
}