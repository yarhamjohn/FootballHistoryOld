namespace football.history.api.Tests.Builders;

[TestFixture]
public class SeasonBuilderTests
{
    [Test]
    public void BuildAllSeasons_returns_empty_array_given_no_seasons()
    {
        var mockSeasonRepository = new Mock<ISeasonRepository>();
        mockSeasonRepository
            .Setup(x => x.GetAllSeasons())
            .Returns(Array.Empty<SeasonModel>());

        var builder = new SeasonBuilder(mockSeasonRepository.Object);

        var seasons = builder.BuildAllSeasons();

        Assert.That(seasons, Is.Empty);
    }
        
    [Test]
    public void BuildAllSeasons_returns_domain_objects_given_models()
    {
        var models = new[]
        {
            new SeasonModel(Id: 1, StartYear: 2000, EndYear: 2001)
        };
            
        var mockSeasonRepository = new Mock<ISeasonRepository>();
        mockSeasonRepository
            .Setup(x => x.GetAllSeasons())
            .Returns(models);

        var builder = new SeasonBuilder(mockSeasonRepository.Object);

        var seasons = builder.BuildAllSeasons();

        Assert.That(seasons.Count, Is.EqualTo(1));

        var actualSeason = seasons.Single();
        var expectedSeasonModel = models.Single();
            
        Assert.That(actualSeason.Id, Is.EqualTo(expectedSeasonModel.Id));
        Assert.That(actualSeason.StartYear, Is.EqualTo(expectedSeasonModel.StartYear));
        Assert.That(actualSeason.EndYear, Is.EqualTo(expectedSeasonModel.EndYear));
    }
        
    [Test]
    public void BuildSeason_returns_null_given_null_model()
    {
        var mockSeasonRepository = new Mock<ISeasonRepository>();
        mockSeasonRepository
            .Setup(x => x.GetSeason(It.IsAny<long>()))
            .Returns((SeasonModel?) null);

        var builder = new SeasonBuilder(mockSeasonRepository.Object);

        var season = builder.BuildSeason(1);

        Assert.That(season, Is.Null);
    }
        
    [Test]
    public void BuildSeason_does_not_handle_repository_exception()
    {
        var exception = new DataInvalidException("An error occurred");

        var mockSeasonRepository = new Mock<ISeasonRepository>();
        mockSeasonRepository
            .Setup(x => x.GetSeason(It.IsAny<long>()))
            .Throws(exception);

        var builder = new SeasonBuilder(mockSeasonRepository.Object);

        var ex = Assert.Throws<DataInvalidException>(() => builder.BuildSeason(1));

        Assert.That(ex, Is.EqualTo(exception));
    }

    [Test]
    public void BuildSeason_returns_domain_object_given_model()
    {
        var expectedSeasonModel = new SeasonModel(Id: 1, StartYear: 2000, EndYear: 2001);
            
        var mockSeasonRepository = new Mock<ISeasonRepository>();
        mockSeasonRepository
            .Setup(x => x.GetSeason(1))
            .Returns(expectedSeasonModel);

        var builder = new SeasonBuilder(mockSeasonRepository.Object);

        var actualSeason = builder.BuildSeason(1);

        Assert.That(actualSeason, Is.Not.Null);

        Assert.That(actualSeason!.Id, Is.EqualTo(expectedSeasonModel.Id));
        Assert.That(actualSeason.StartYear, Is.EqualTo(expectedSeasonModel.StartYear));
        Assert.That(actualSeason.EndYear, Is.EqualTo(expectedSeasonModel.EndYear));
    }
}