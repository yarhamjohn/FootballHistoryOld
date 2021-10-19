using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Match
{
    public interface IHistoricalSeasonCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long teamId, long[] seasonIds);
    }

    public class HistoricalSeasonCommandBuilder : IHistoricalSeasonCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long teamId, long[] seasonIds)
        {
            var whereClause = BuildWhereClause(seasonIds);
            var sql = GetSql(whereClause);
            var cmd = BuildCommand(connection, sql);
            
            AddParameters(cmd, teamId, seasonIds);

            return cmd;
        }

        private string BuildWhereClause(long[] seasonIds)
            => seasonIds.Any() 
                ? $"WHERE s.Id IN ({string.Join(",", seasonIds.Select((_, i) => $"@SeasonId{i}"))})"
                : "WHERE 1 = 0";

        private static void AddParameters(
            DbCommand cmd,
            long teamId,
            long[] seasonIds)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@TeamId",
                    Value         = teamId
                });

            for (var i = 0; i < seasonIds.Length; i++)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = $"SeasonId{i}",
                        Value = seasonIds[i]
                    });
            }
        }

        private static DbCommand BuildCommand(IDatabaseConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        private static string GetSql(string whereClause) => $@"
SELECT 
       s.Id AS SeasonId, 
       StartYear, 
       c.Id AS CompetitionId, 
       c.Name, 
       c.Tier, 
       cr.TotalPlaces, 
       p.Position, 
       p.Status 
FROM dbo.Seasons AS s
LEFT JOIN dbo.Competitions AS c 
    ON c.SeasonId = s.Id
LEFT JOIN dbo.CompetitionRules AS cr 
    on cr.Id = c.RulesId
LEFT JOIN dbo.Positions AS p 
    on p.CompetitionId = c.Id AND p.TeamId = @TeamId
{whereClause}
";
    }
}