using football.history.api.Domain;
using football.history.api.Exceptions;
using football.history.api.Models;
using football.history.api.Repositories;

namespace football.history.api.Builders;

public interface ILeagueTableBuilder
{
    LeagueTable? BuildFullLeagueTable(long competitionId);
    LeagueTable? BuildFullLeagueTable(long seasonId, long teamId);
    LeagueTable BuildPartialLeagueTable(
        CompetitionModel competition,
        MatchModel[] leagueMatches,
        DateTime targetDate,
        PointDeductionModel[] pointDeductions);
}
    
public class LeagueTableBuilder : ILeagueTableBuilder
{
    private readonly IRowComparerFactory _rowComparerFactory;
    private readonly ICompetitionRepository _competitionRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPointDeductionRepository _pointDeductionRepository;
    private readonly IRowBuilder _rowBuilder;

    public LeagueTableBuilder(
        IMatchRepository matchRepository,
        IPointDeductionRepository pointDeductionRepository,
        IRowBuilder rowBuilder,
        IPositionRepository positionRepository,
        IRowComparerFactory rowComparerFactory,
        ICompetitionRepository competitionRepository)
    {
        _matchRepository = matchRepository;
        _pointDeductionRepository = pointDeductionRepository;
        _rowBuilder = rowBuilder;
        _positionRepository = positionRepository;
        _rowComparerFactory = rowComparerFactory;
        _competitionRepository = competitionRepository;
    }
        
    public LeagueTable? BuildFullLeagueTable(long competitionId)
    {
        var competition = _competitionRepository.GetCompetition(competitionId);
        
        var leagueMatches = _matchRepository.GetLeagueMatches(competition.Id);
        var teamsInLeague = GetTeamsInLeague(leagueMatches);
        var pointDeductions = _pointDeductionRepository.GetPointDeductions(competition.Id);

        var rows = GetRows(competition, teamsInLeague, leagueMatches, pointDeductions);
            
        var positions = _positionRepository.GetCompetitionPositions(competition.Id);

        foreach (var row in rows)
        {
            row.Position = positions.Single(x => x.TeamId == row.TeamId).LeaguePosition;
            row.Status = positions.Single(x => x.TeamId == row.TeamId).Status;
        }
            
        return new LeagueTable(rows, ToCompetitionDomain(competition));
    }

    public LeagueTable? BuildFullLeagueTable(long seasonId, long teamId)
    {
        var competition = _competitionRepository.GetTeamCompetition(seasonId, teamId);
        if (competition is null)
        {
            throw new DataNotFoundException($"No competition was found for the specified seasonId ({seasonId}) and teamId ({teamId}).");
        }

        return BuildFullLeagueTable(competition.Id);
    }

    public LeagueTable BuildPartialLeagueTable(CompetitionModel competition, MatchModel[] leagueMatches, DateTime targetDate, PointDeductionModel[] pointDeductions)
    {
        var matchesToDate = leagueMatches.Where(x => x.MatchDate < targetDate).ToArray();
        var teamsInLeague = GetTeamsInLeague(leagueMatches);

        var rows = GetRows(competition, teamsInLeague, matchesToDate, pointDeductions);

        SetPositions(competition, rows.ToList());
            
        return new LeagueTable(rows, ToCompetitionDomain(competition));
    }

    private static IEnumerable<TeamModel> GetTeamsInLeague(MatchModel[] leagueMatches)
    {
        return leagueMatches.SelectMany(
                m => new[]
                {
                    new TeamModel(m.HomeTeamId, m.HomeTeamName, m.HomeTeamAbbreviation, Notes: null),
                    new TeamModel(m.AwayTeamId, m.AwayTeamName, m.AwayTeamAbbreviation, Notes: null)
                })
            .Distinct();
    }
        
    private LeagueTableRow[] GetRows(
        CompetitionModel competition,
        IEnumerable<TeamModel> teamsInLeague,
        MatchModel[] leagueMatches,
        PointDeductionModel[] pointDeductions)
    {
        return teamsInLeague
            .Select(team => _rowBuilder.Build(competition, team, leagueMatches, pointDeductions))
            .ToArray();
    }

    private void SetPositions(CompetitionModel competition, List<LeagueTableRow> rows)
    {
        var leagueTableComparer = _rowComparerFactory.GetLeagueTableComparer(competition);
        rows.Sort(leagueTableComparer);

        for (var i = 0; i < rows.Count; i++)
        {
            rows[i].Position = rows.Count - i;
        }
    }

    private static Competition ToCompetitionDomain(CompetitionModel competition) =>
        new(competition.Id,
            competition.Name,
            Season: new(
                competition.SeasonId,
                competition.StartYear,
                competition.EndYear),
            competition.Level,
            competition.Comment,
            Rules: new(
                competition.PointsForWin,
                competition.TotalPlaces,
                competition.PromotionPlaces,
                competition.RelegationPlaces,
                competition.PlayOffPlaces,
                competition.RelegationPlayOffPlaces,
                competition.ReElectionPlaces,
                competition.FailedReElectionPosition));
}