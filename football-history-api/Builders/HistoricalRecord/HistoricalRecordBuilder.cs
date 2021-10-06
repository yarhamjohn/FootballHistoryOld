using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Season;
using football.history.api.Repositories.Team;

namespace football.history.api.Builders
{
    public interface IHistoricalRecordBuilder
    {
        HistoricalRecord Build(long teamId, long[] seasonIds);
    }

    public class HistoricalRecordBuilder : IHistoricalRecordBuilder
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IPositionRepository _positionRepository;

        public HistoricalRecordBuilder(
            ISeasonRepository seasonRepository,
            ICompetitionRepository competitionRepository,
            IPositionRepository positionRepository)
        {
            _seasonRepository = seasonRepository;
            _competitionRepository = competitionRepository;
            _positionRepository = positionRepository;
        }

        public HistoricalRecord Build(long teamId, long[] seasonIds)
        {
            var historicalSeasons = seasonIds
                .Select(s => BuildHistoricalSeasons(teamId, s))
                .ToArray();

            return new HistoricalRecord(teamId, historicalSeasons);
        }

        private HistoricalSeason BuildHistoricalSeasons(long teamId, long seasonId)
        {
            //TODO: reduce these db calls. There are 4 in this method call. Can they be moved upwards and done in bulk?
            var (_, startYear, _) = _seasonRepository.GetSeason(seasonId);

            var tierPlaces = _competitionRepository.GetCompetitionsInSeason(seasonId)
                .Select(x => (x.Tier, x.TotalPlaces))
                .ToArray();

            var boundaries = BuildBoundaries(tierPlaces);

            try
            {
                var competition = _competitionRepository.GetCompetitionForSeasonAndTeam(seasonId, teamId);
                var historicalPosition = BuildHistoricalPosition(teamId, competition, tierPlaces);

                return new HistoricalSeason(seasonId, startYear, boundaries, historicalPosition);
            }
            catch (DataNotFoundException)
            {
                return new HistoricalSeason(seasonId, startYear, boundaries, null);
            }
        }

        private HistoricalPosition BuildHistoricalPosition(
            long teamId,
            CompetitionModel competition,
            IEnumerable<(int Tier, int TotalPlaces)> tierPlaces)
        {
            var position = _positionRepository.GetPosition(competition.Id, teamId);
            var overallPosition = GetOverallPosition(tierPlaces, competition.Tier, position.Position);

            return new HistoricalPosition(
                competition.Id,
                competition.Name,
                position.Position,
                overallPosition,
                position.Status);
        }

        private static int GetOverallPosition(
            IEnumerable<(int Tier, int TotalPlaces)> tierPlaces, int tier, int position)
            => position + tierPlaces
                .Where(x => x.Tier < tier)
                .Select(y => y.TotalPlaces)
                .Sum();

        private static int[] BuildBoundaries((int Tier, int TotalPlaces)[] tierPlaces)
        {
            var boundary = 0;

            var containsNorthSouth = tierPlaces.Count(x => x.Tier == 3) > 1;
            if (!containsNorthSouth)
            {
                return tierPlaces
                    .OrderBy(x => x.Tier)
                    .Select(y => boundary += y.TotalPlaces)
                    .ToArray();
            }

            var boundaries = tierPlaces
                .Where(x => x.Tier < 3)
                .OrderBy(x => x.Tier)
                .Select(y => boundary += y.TotalPlaces)
                .ToList();

            var northSouthBoundaries = tierPlaces.Where(x => x.Tier == 3).Select(y => boundary + y.TotalPlaces);
            boundaries.AddRange(northSouthBoundaries);

            return boundaries.ToArray();
        }
    }
}