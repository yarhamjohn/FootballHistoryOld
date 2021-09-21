using System.Collections.Generic;
using System.Data.Common;

namespace football.history.api.Repositories.Team
{
    public interface IPositionRepository
    {
        List<PositionModel> GetCompetitionPositions(long competitionId);
        List<PositionModel> GetTeamPositions(long teamId);
    }

    public class PositionRepository : IPositionRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly IPositionCommandBuilder _queryBuilder;

        public PositionRepository(IDatabaseConnection connection, IPositionCommandBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public List<PositionModel> GetCompetitionPositions(long competitionId)
        {
            _connection.Open();
            var cmd = _queryBuilder.BuildForCompetition(_connection, competitionId);
            var positions = GetPositionModels(cmd);
            _connection.Close();

            return positions;
        }

        public List<PositionModel> GetTeamPositions(long teamId)
        {
            _connection.Open();
            var cmd = _queryBuilder.BuildForTeam(_connection, teamId);
            var positions = GetPositionModels(cmd);
            _connection.Close();

            return positions;
        }

        private static List<PositionModel> GetPositionModels(DbCommand cmd)
        {
            var positions = new List<PositionModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                positions.Add(GetPositionModel(reader));
            }

            return positions;
        }

        private static PositionModel GetPositionModel(DbDataReader reader)
            => new(
                Id: reader.GetInt64(0),
                CompetitionId: reader.GetInt64(1),
                TeamId: reader.GetInt64(2),
                Position: reader.GetByte(3),
                Status: reader.IsDBNull(4) ? null : reader.GetString(4));
    }
}