using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Team;

public interface IPositionCommandBuilder
{
    public DbCommand Build(IDatabaseConnection connection, long? competitionId, long? teamId);
}

public class PositionCommandBuilder : IPositionCommandBuilder
{
    public DbCommand Build(IDatabaseConnection connection, long? competitionId, long? teamId)
    {
        var whereClause = BuildWhereClause(competitionId, teamId);
        var sql = GetSql(whereClause);
            
        var cmd = BuildCommand(connection, sql);
        AddParameters(cmd, competitionId, teamId);
            
        return cmd;
    }

    private static string GetSql(string whereClause)
    {
        return $@"
SELECT p.Id, p.CompetitionId, c.Name, p.TeamId, t.Name, p.Position, p.Status
FROM [dbo].[Positions] AS p
LEFT JOIN [dbo].[Teams] AS t ON t.Id = p.TeamId
LEFT JOIN [dbo].[Competitions] AS c ON c.Id = p.CompetitionId
{whereClause}
";
    }

    private string BuildWhereClause(long? competitionId, long? teamId)
    {
        var clauses = new List<string>();

        if (competitionId is not null)
        {
            clauses.Add("p.CompetitionId = @CompetitionId");
        }

        if (teamId is not null)
        {
            clauses.Add("p.TeamId = @TeamId");
        }
            
        return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : "";
    }
        
        
    private static void AddParameters(
        DbCommand cmd,
        long? competitionId,
        long? teamId)
    {
        if (competitionId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@CompetitionId",
                    Value         = competitionId
                });
        }

        if (teamId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@TeamId",
                    Value         = teamId
                });
        }
    }
        
    private static DbCommand BuildCommand(IDatabaseConnection connection, string sql)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        return cmd;
    }
}