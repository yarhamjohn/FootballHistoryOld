using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories;

public interface IPositionRepository
{
    /// <summary>
    /// Retrieves models for each position in the given <paramref name="competitionId" />.
    /// </summary>
    ///
    /// <param name="competitionId">
    /// The id of the required competition.
    /// </param>
    /// 
    /// <returns>
    /// A collection of <see cref="PositionModel">PositionModels</see>.
    /// Can be empty if the <paramref name="competitionId"/> matched no competition.
    /// </returns>
    PositionModel[] GetCompetitionPositions(long competitionId);
    
    /// <summary>
    /// Retrieves models for each position the given <paramref name="teamId" />
    /// has finished in across all competitions.
    /// </summary>
    ///
    /// <param name="teamId">
    /// The id of the required team.
    /// </param>
    /// 
    /// <returns>
    /// A collection of <see cref="PositionModel">PositionModels</see>.
    /// Can be empty if the <paramref name="teamId"/> matched no team or the
    /// team has never been positioned in a competition.
    /// </returns>
    PositionModel[] GetTeamPositions(long teamId);
}

public class PositionRepository : IPositionRepository
{
    private readonly IDatabaseConnection _connection;

    public PositionRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public PositionModel[] GetCompetitionPositions(long competitionId)
    {
        _connection.Open();

        var cmd = BuildCommand(_connection, competitionId, null);
        var positions = GetPositionModels(cmd);

        _connection.Close();

        return positions;
    }

    public PositionModel[] GetTeamPositions(long teamId)
    {
        _connection.Open();

        var cmd = BuildCommand(_connection, null, teamId);
        var positions = GetPositionModels(cmd);

        _connection.Close();

        return positions;
    }

    private static PositionModel[] GetPositionModels(DbCommand cmd)
    {
        var positions = new List<PositionModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            positions.Add(GetPositionModel(reader));
        }

        return positions.ToArray();
    }

    private static PositionModel GetPositionModel(DbDataReader reader)
        => new(
            Id: reader.GetInt64(0),
            CompetitionId: reader.GetInt64(1),
            CompetitionName: reader.GetString(2),
            TeamId: reader.GetInt64(3),
            TeamName: reader.GetString(4),
            LeaguePosition: reader.GetByte(5),
            Status: reader.IsDBNull(6) ? null : reader.GetString(6));

    private static DbCommand BuildCommand(IDatabaseConnection connection, long? competitionId, long? teamId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql(competitionId, teamId);

        AddParameters(cmd, competitionId, teamId);

        return cmd;
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
                    Value = competitionId
                });
        }

        if (teamId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@TeamId",
                    Value = teamId
                });
        }
    }

    private static string GetSql(long? competitionId, long? teamId)
        => $@"
            SELECT 
                   p.Id,
                   p.CompetitionId,
                   c.Name,
                   p.TeamId,
                   t.Name,
                   p.Position,
                   p.Status
            FROM [dbo].[Positions] AS p
            LEFT JOIN [dbo].[Teams] AS t
                ON t.Id = p.TeamId
            LEFT JOIN [dbo].[Competitions] AS c
                ON c.Id = p.CompetitionId
            {BuildWhereClause(competitionId, teamId)}
            ";

    private static string BuildWhereClause(long? competitionId, long? teamId)
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
}