namespace football.history.api.Builders;

public interface ISeasonBuilder
{
    /// <summary>
    /// Constructs a collection of all <see cref="Season"/> domain objects.
    /// </summary>
    /// 
    /// <returns>
    /// Returns a collection of all <see cref="Season"/> domain objects.
    /// </returns>
    Season[] BuildAllSeasons();

    /// <summary>
    /// Constructs a <see cref="Season"/> domain object.
    /// </summary>
    /// 
    /// <param name="seasonId">
    /// The id of the required season.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="seasonId"/> matched more than one season.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="Season"/> domain object or
    /// null if the given <paramref name="seasonId"/> matched no season.
    /// </returns>
    Season? BuildSeason(long seasonId);
}

public class SeasonBuilder : ISeasonBuilder
{
    private readonly ISeasonRepository _repository;

    public SeasonBuilder(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public Season[] BuildAllSeasons()
        => _repository.GetAllSeasons()
            .Select(ToDomain)
            .ToArray();

    public Season? BuildSeason(long seasonId)
    {
        var model = _repository.GetSeason(seasonId);
        return model is null ? null : ToDomain(model);
    }

    private static Season ToDomain(SeasonModel model)
        => new(model.Id, model.StartYear, model.EndYear);
}