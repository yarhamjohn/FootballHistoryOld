using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using football.history.api.Builders;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
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

        [Test]
        public async Task GetHistoricalRecord_returns_bad_request_given_no_season_ids()
        {
            var client = GetTestClient();

            var response = await client.GetAsync("api/v2/historical-record/teamId/1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString, Is.EqualTo("No seasonIds were specified."));
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
            
            mockHistoricalRecordBuilder.VerifyAll();
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
        
        private static HttpClient GetTestClient()
        {
            var factory = new WebApplicationFactory<Startup>();
            return factory.CreateClient();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void SwapTransient<TService>(
            this IServiceCollection serviceCollection,
            TService mockImplementation)
        {
            var serviceDescriptors = serviceCollection
                .Where(descriptor =>
                    descriptor.ServiceType == typeof(TService)
                    && descriptor.Lifetime == ServiceLifetime.Transient)
                .ToArray();

            foreach (var serviceDescriptor in serviceDescriptors)
            {
                serviceCollection.Remove(serviceDescriptor);
            }

            serviceCollection.AddTransient(typeof(TService),_ => mockImplementation!);
        }
    }
}