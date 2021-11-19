using System.Data.Common;
using football.history.api.Models;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories;

public interface IHistoricalSeasonRepository
{
    /// <summary>
    /// Retrieves models for each historical season that matches
    /// the provided <paramref name="teamId"/> and <paramref name="seasonIds"/>.
    /// </summary>
    ///
    /// <param name="teamId">
    /// The id of the required team.
    /// </param>
    /// 
    /// <param name="seasonIds">
    /// A collection of ids for the required seasons.
    /// </param>
    /// 
    /// <returns>
    /// A collection of <see cref="HistoricalSeasonModel">HistoricalSeasonModels</see>.
    /// Can be empty if the <paramref name="teamId"/> matched no team or
    /// none of the <paramref name="seasonIds"/> matches a season. 
    /// </returns>
    public List<HistoricalSeasonModel> GetHistoricalSeasons(long teamId, long[] seasonIds);
}

public class HistoricalSeasonRepository : IHistoricalSeasonRepository
{
    private readonly IDatabaseConnection _connection;

    public HistoricalSeasonRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public List<HistoricalSeasonModel> GetHistoricalSeasons(long teamId, long[] seasonIds)
    {
        _connection.Open();

        var cmd = BuildCommand(_connection, teamId, seasonIds);
        var seasons = GetHistoricalSeasonModels(cmd);

        _connection.Close();

        return seasons;
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

    private static DbCommand BuildCommand(IDatabaseConnection connection, long teamId, long[] seasonIds)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql(seasonIds, teamId);

        AddParameters(cmd, teamId, seasonIds);

        return cmd;
    }

    private static void AddParameters(DbCommand cmd, long teamId, long[] seasonIds)
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
                    Value         = seasonIds[i]
                });
        }
    }

    private static string GetSql(long[] seasonIds, long teamId)
        => $@"
                SELECT 
                       s.Id AS SeasonId, 
                       s.StartYear, 
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
                {BuildWhereClause(seasonIds, teamId)}
                ";

    private static string BuildWhereClause(long[] seasonIds, long teamId)
        => seasonIds.Any()
            ? $@"WHERE s.Id IN ({string.Join(",", seasonIds.Select((_, i) => $"@SeasonId{i}"))})
                        AND EXISTS (SELECT Id FROM dbo.Teams AS t WHERE t.Id = {teamId})"
            : "WHERE 1 = 0";
}