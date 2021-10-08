using System.Collections.Generic;
using System.Data.Common;

namespace football.history.api.Repositories.Match
{
    public interface IHistoricalSeasonRepository
    {
        public List<HistoricalSeasonModel> GetHistoricalSeasons(long teamId, long[] seasonIds);
    }

    public class HistoricalSeasonRepository : IHistoricalSeasonRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly IHistoricalSeasonCommandBuilder _queryBuilder;

        public HistoricalSeasonRepository(IDatabaseConnection connection, IHistoricalSeasonCommandBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public List<HistoricalSeasonModel> GetHistoricalSeasons(long teamId, long[] seasonIds)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, teamId, seasonIds);
            var historicalSeasons = GetHistoricalSeasonModels(cmd);
            _connection.Close();

            return historicalSeasons;
        }

        private static List<HistoricalSeasonModel> GetHistoricalSeasonModels(DbCommand cmd)
        {
            var historicalSeasons = new List<HistoricalSeasonModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                historicalSeasons.Add(GetHistoricalSeasonModel(reader));
            }

            return historicalSeasons;
        }

        private static HistoricalSeasonModel GetHistoricalSeasonModel(DbDataReader reader)
            => new(
                SeasonId: reader.GetInt64(0),
                StartYear: reader.GetInt16(1),
                PositionModel: reader.IsDBNull(2)
                    ? null
                    : new HistoricalPositionModel(
                        CompetitionId: reader.GetInt64(2),
                        CompetitionName: reader.GetString(3),
                        Tier: reader.GetByte(4),
                        TotalPlaces: reader.GetByte(5),
                        Position: reader.IsDBNull(6) ? null : reader.GetByte(6),
                        Status: reader.IsDBNull(7) ? null : reader.GetString(7)));
    }
}