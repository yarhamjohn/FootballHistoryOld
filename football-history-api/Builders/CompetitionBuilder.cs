using football.history.api.Domain;
using football.history.api.Models;
using football.history.api.Repositories;

namespace football.history.api.Builders;

public interface ICompetitionBuilder
{
    /// <summary>
    /// Constructs a collection of <see cref="Competition"/> domain objects.
    /// </summary>
    ///
    /// <param name="seasonId">
    /// Optional.
    /// Specifies which season to limit the returned <see cref="Competition"/>
    /// domain objects to. If unset, returns all.
    /// </param>
    /// 
    /// <returns>
    /// Returns a collection of <see cref="Competition"/> domain objects.
    /// </returns>
    Competition[] BuildCompetitions(long? seasonId = null);

    /// <summary>
    /// Constructs a <see cref="Competition"/> domain object.
    /// </summary>
    /// 
    /// <param name="competitionId">
    /// The id of the required competition.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="competitionId"/>
    /// matched more than one competition.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="Competition"/> domain object or
    /// null if the given <paramref name="competitionId"/> matched no competition.
    /// </returns>
    Competition? BuildCompetition(long competitionId);
}

public class CompetitionBuilder : ICompetitionBuilder
{
    private readonly ICompetitionRepository _repository;

    public CompetitionBuilder(ICompetitionRepository repository)
    {
        _repository = repository;
    }

    public Competition[] BuildCompetitions(long? seasonId = null)
        => _repository.GetCompetitions(seasonId)
            .Select(ToDomain)
            .ToArray();

    public Competition? BuildCompetition(long competitionId)
    {
        var model = _repository.GetCompetition(competitionId);
        return model is null ? null : ToDomain(model);
    }

    private static Competition ToDomain(CompetitionModel model)
    =>
        new(model.Id,
            model.Name,
            Season: new(
                model.SeasonId,
                model.StartYear,
                model.EndYear),
            model.Level,
            model.Comment,
            Rules: new(
                model.PointsForWin,
                model.TotalPlaces,
                model.PromotionPlaces,
                model.RelegationPlaces,
                model.PlayOffPlaces,
                model.RelegationPlayOffPlaces,
                model.ReElectionPlaces,
                model.FailedReElectionPosition));
}