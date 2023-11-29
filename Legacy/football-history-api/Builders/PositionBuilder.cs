namespace football.history.api.Builders;

public interface IPositionBuilder
{
    Position[] BuildSeasonPositions(long seasonId);
    Position[] BuildTeamPositions(long teamId);
}

public class PositionBuilder : IPositionBuilder
{
    private readonly IPositionRepository _positionRepository;
    private readonly ICompetitionRepository _competitionRepository;

    public PositionBuilder(IPositionRepository positionRepository, ICompetitionRepository competitionRepository)
    {
        _positionRepository = positionRepository;
        _competitionRepository = competitionRepository;
    }

    public Position[] BuildSeasonPositions(long seasonId)
    {
        var competitions = _competitionRepository.GetCompetitions(seasonId);

        return competitions
            .SelectMany(competition =>
                _positionRepository.GetCompetitionPositions(competition.Id))
            .Select(ToDomain)
            .ToArray();
    }

    public Position[] BuildTeamPositions(long teamId)
        => _positionRepository.GetTeamPositions(teamId).Select(ToDomain).ToArray();

    private static Position ToDomain(PositionModel position) =>
        new(position.Id,
            position.CompetitionId,
            position.CompetitionName,
            position.TeamId,
            position.TeamName,
            position.LeaguePosition,
            position.Status);
}