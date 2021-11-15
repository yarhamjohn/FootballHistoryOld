using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Models;
using football.history.api.Repositories;

namespace football.history.api.Builders;

public interface ILeagueTableBuilder
{
    ILeagueTable BuildFullLeagueTable(CompetitionModel competition);
    ILeagueTable BuildPartialLeagueTable(
        CompetitionModel competition,
        MatchModel[] leagueMatches,
        DateTime targetDate,
        PointDeductionModel[] pointDeductions);
}
    
public class LeagueTableBuilder : ILeagueTableBuilder
{
    private readonly IRowComparerFactory _rowComparerFactory;
    private readonly IPositionRepository _positionRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPointDeductionRepository _pointDeductionRepository;
    private readonly IRowBuilder _rowBuilder;

    public LeagueTableBuilder(
        IMatchRepository matchRepository,
        IPointDeductionRepository pointDeductionRepository,
        IRowBuilder rowBuilder,
        IPositionRepository positionRepository,
        IRowComparerFactory rowComparerFactory)
    {
        _matchRepository = matchRepository;
        _pointDeductionRepository = pointDeductionRepository;
        _rowBuilder = rowBuilder;
        _positionRepository = positionRepository;
        _rowComparerFactory = rowComparerFactory;
    }
        
    public ILeagueTable BuildFullLeagueTable(CompetitionModel competition)
    {
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
            
        return new LeagueTable(rows);
    }
        
    public ILeagueTable BuildPartialLeagueTable(CompetitionModel competition, MatchModel[] leagueMatches, DateTime targetDate, PointDeductionModel[] pointDeductions)
    {
        var matchesToDate = leagueMatches.Where(x => x.MatchDate < targetDate).ToArray();
        var teamsInLeague = GetTeamsInLeague(leagueMatches);

        var rows = GetRows(competition, teamsInLeague, matchesToDate, pointDeductions);

        SetPositions(competition, rows);
            
        return new LeagueTable(rows);
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
        
    private List<LeagueTableRowDto> GetRows(
        CompetitionModel competition,
        IEnumerable<TeamModel> teamsInLeague,
        MatchModel[] leagueMatches,
        PointDeductionModel[] pointDeductions)
    {
        return teamsInLeague
            .Select(team => _rowBuilder.Build(competition, team, leagueMatches, pointDeductions))
            .ToList();
    }

    private void SetPositions(CompetitionModel competition, List<LeagueTableRowDto> rows)
    {
        var leagueTableComparer = _rowComparerFactory.GetLeagueTableComparer(competition);
        rows.Sort(leagueTableComparer);

        for (var i = 0; i < rows.Count; i++)
        {
            rows[i].Position = rows.Count - i;
        }
    }
}