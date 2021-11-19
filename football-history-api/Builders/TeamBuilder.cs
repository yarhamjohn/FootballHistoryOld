using football.history.api.Models;
using football.history.api.Domain;
using football.history.api.Repositories;

namespace football.history.api.Builders;

public interface ITeamBuilder
{
    /// <summary>
    /// Constructs a collection of all <see cref="Team"/> domain objects.
    /// </summary>
    /// 
    /// <returns>
    /// Returns a collection of all <see cref="Team"/> domain objects.
    /// </returns>
    Team[] BuildAllTeams();

    /// <summary>
    /// Constructs a <see cref="Team"/> domain object.
    /// </summary>
    /// 
    /// <param name="teamId">
    /// The id of the required team.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="teamId"/> matched more than one team.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="Team"/> domain object or
    /// null if the given <paramref name="teamId"/> matched no team.
    /// </returns>
    Team? BuildTeam(long teamId);
}

public class TeamBuilder : ITeamBuilder
{
    private readonly ITeamRepository _repository;

    public TeamBuilder(ITeamRepository repository)
    {
        _repository = repository;
    }

    public Team[] BuildAllTeams()
        => _repository.GetAllTeams()
            .Select(ToDomain)
            .ToArray();

    public Team? BuildTeam(long teamId)
    {
        var model = _repository.GetTeam(teamId);
        return model is null ? null : ToDomain(model);
    }

    private static Team ToDomain(TeamModel model)
        => new(model.Id, model.Name, model.Abbreviation, model.Notes);
}