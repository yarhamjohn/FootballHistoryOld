using System.Collections.Generic;
using System.Linq;
using football.history.api.Domain;
using football.history.api.Exceptions;
using football.history.api.Bindings;
using football.history.api.Repositories;

namespace football.history.api.Builders
{
    public interface IHistoricalRecordBuilder
    {
        /// <summary>
        /// Constructs a <see cref="HistoricalRecord"/> domain object.
        /// </summary>
        ///
        /// <exception cref="DataInvalidException">
        /// Thrown if the given <paramref name="teamId"/> appeared in
        /// multiple competitions in a single season or if any season
        /// contained multiple competitions of which any were undefined
        /// (seasons with single, undefined competitions are expected due
        /// to World War One and World War Two).
        /// </exception>
        HistoricalRecord Build(long teamId, long[] seasonIds);
    }

    public class HistoricalRecordBuilder : IHistoricalRecordBuilder
    {
        private readonly IHistoricalSeasonRepository _historicalSeasonRepository;

        public HistoricalRecordBuilder(IHistoricalSeasonRepository historicalSeasonRepository)
        {
            _historicalSeasonRepository = historicalSeasonRepository;
        }

        public HistoricalRecord Build(long teamId, long[] seasonIds)
        {
            var historicalSeasonModels = _historicalSeasonRepository.GetHistoricalSeasons(teamId, seasonIds);

            var groupedSeasons = historicalSeasonModels
                .GroupBy(x => (x.SeasonId, x.StartYear));

            var historicalSeasons = groupedSeasons
                .Select(BuildHistoricalSeason)
                .ToArray();

            return new HistoricalRecord(teamId, historicalSeasons);
        }

        private static HistoricalSeason BuildHistoricalSeason(
            IGrouping<(long SeasonId, int StartYear), HistoricalSeasonModel> seasonGroup)
        {
            var historicalSeason = new HistoricalSeason(seasonGroup.Key.SeasonId, seasonGroup.Key.StartYear);

            var positionModels = seasonGroup.Select(x => x.PositionModel).ToArray();

            return positionModels.Length switch
            {
                1 when positionModels.Single() is null => historicalSeason,
                > 1 when positionModels.Any(x => x is null)
                    => throw new DataInvalidException(
                        "Some competitions were null in the given season " +
                        $"(id: {seasonGroup.Key.SeasonId}, startYear: {seasonGroup.Key.StartYear})."),
                _ => historicalSeason with
                {
                    Boundaries = BuildBoundaries(positionModels!),
                    HistoricalPosition = BuildHistoricalPosition(positionModels!)
                }
            };
        }

        private static HistoricalPosition? BuildHistoricalPosition(
            IReadOnlyCollection<HistoricalPositionModel> positionModels)
        {
            var modelsWithPositions = positionModels
                .Where(x => x.Position is not null)
                .ToArray();

            if (!modelsWithPositions.Any())
            {
                return null;
            }

            if (modelsWithPositions.Length > 1)
            {
                throw new DataInvalidException(
                    "The team was assigned positions in more than one competition in a season.");
            }

            var activeCompetition = modelsWithPositions.Single();

            return new HistoricalPosition(
                activeCompetition.CompetitionId,
                activeCompetition.CompetitionName,
                (int)activeCompetition.Position!,
                GetOverallPosition(positionModels, activeCompetition.Tier, (int)activeCompetition.Position!),
                activeCompetition.Status);
        }

        private static int[] BuildBoundaries(IReadOnlyCollection<HistoricalPositionModel> seasonModel)
        {
            var boundary = 0;

            if (!ContainsNorthSouth(seasonModel))
            {
                return seasonModel
                    .OrderBy(x => x.Tier)
                    .Select(y => boundary += y.TotalPlaces)
                    .ToArray();
            }

            var boundaries = seasonModel
                .Where(x => x.Tier < 3)
                .OrderBy(x => x.Tier)
                .Select(y => boundary += y.TotalPlaces)
                .ToList();

            var northSouthBoundaries = seasonModel
                .Where(x => x.Tier == 3)
                .Select(y => boundary + y.TotalPlaces);

            boundaries.AddRange(northSouthBoundaries);

            return boundaries.ToArray();
        }

        private static bool ContainsNorthSouth(IReadOnlyCollection<HistoricalPositionModel> seasonModel)
            => seasonModel.Count(x => x.CompetitionName
                is "Third Division North"
                or "Third Division South") == 2;

        private static int GetOverallPosition(
            IEnumerable<HistoricalPositionModel> positionModels, int tier, int position)
            => position + positionModels
                .Where(x => x.Tier < tier)
                .Select(y => y.TotalPlaces)
                .Sum();
    }
}