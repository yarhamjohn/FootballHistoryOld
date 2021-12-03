namespace football.history.api.Builders
{
    public interface ILeaguePositionBuilder
    {
        LeaguePosition[] GetPositions(long competitionId, long teamId);
    }

    public class LeaguePositionBuilder : ILeaguePositionBuilder
    {
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointDeductionRepository _pointDeductionRepository;
        private readonly ICompetitionRepository _competitionRepository;

        public LeaguePositionBuilder(
            ILeagueTableBuilder leagueTableBuilder,
            IMatchRepository matchRepository,
            IPointDeductionRepository pointDeductionRepository,
            ICompetitionRepository competitionRepository)
        {
            _leagueTableBuilder       = leagueTableBuilder;
            _matchRepository          = matchRepository;
            _pointDeductionRepository = pointDeductionRepository;
            _competitionRepository    = competitionRepository;
        }

        public LeaguePosition[] GetPositions(long competitionId, long teamId)
        {
            var competition = _competitionRepository.GetCompetition(competitionId);
            if (competition is null)
            {
                return Array.Empty<LeaguePosition>();
            }

            var leagueMatches = _matchRepository.GetLeagueMatches(competition.Id);
            if (!leagueMatches.Any())
            {
                return Array.Empty<LeaguePosition>();
            }

            var pointDeductions = _pointDeductionRepository.GetPointDeductions(competition.Id);

            return GetDates(leagueMatches)
                .Select(d => GetLeaguePosition(teamId, competition, leagueMatches, d, pointDeductions))
                .ToArray();
        }

        private LeaguePosition GetLeaguePosition(
            long teamId,
            CompetitionModel competition,
            MatchModel[] matches,
            DateTime date,
            PointDeductionModel[] deductions)
        {
            var partialLeagueTable = _leagueTableBuilder.BuildPartialLeagueTable(competition, matches, date, deductions);
        
            return new(
                Date: date,
                Position: partialLeagueTable.Table.Single(row => row.TeamId == teamId).Position);
        }

        private static IEnumerable<DateTime> GetDates(MatchModel[] leagueMatches)
        {
            var startDate = leagueMatches.Min(x => x.MatchDate).AddDays(-1);
            var endDate = leagueMatches.Max(x => x.MatchDate).AddDays(1);

            return Enumerable
                .Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset)).ToList();
        }
    }
}