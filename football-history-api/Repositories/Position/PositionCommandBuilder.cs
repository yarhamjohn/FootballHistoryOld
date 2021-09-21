using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Team
{
    public interface IPositionCommandBuilder
    {
        public DbCommand BuildForCompetition(IDatabaseConnection connection, long competitionId);
        public DbCommand BuildForTeam(IDatabaseConnection connection, long teamId);
    }

    public class PositionCommandBuilder : IPositionCommandBuilder
    {
        public DbCommand BuildForCompetition(IDatabaseConnection connection, long competitionId)
        {
            const string sql = @"
SELECT p.Id, p.CompetitionId, p.TeamId, p.Position, p.Status
FROM [dbo].[Positions] AS p
WHERE p.CompetitionId = @Id
";

            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Value         = competitionId
                });

            return cmd;
        }

        public DbCommand BuildForTeam(IDatabaseConnection connection, long teamId)
        {
            const string sql = @"
SELECT p.Id, p.CompetitionId, p.TeamId, p.Position, p.Status
FROM [dbo].[Positions] AS p
WHERE p.TeamId = @Id
";

            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Value         = teamId
                });

            return cmd;
        }
        
        private static DbCommand BuildCommand(IDatabaseConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }
    }
}
