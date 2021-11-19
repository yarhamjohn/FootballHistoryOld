using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Models;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories;

public interface ICompetitionRepository
{
    /// <summary>
    /// Retrieves a model from the database for the competition
    /// matching the provided <paramref name="competitionId"/>.
    /// </summary>
    /// 
    /// <param name="competitionId">
    /// The id of the required competition.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="competitionId"/> matched more than one competition.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="CompetitionModel"/> or null if
    /// the given <paramref name="competitionId"/> matched no competition.
    /// </returns>
    CompetitionModel? GetCompetition(long competitionId);
    
    /// <summary>
    /// Retrieves a model from the database for the competition
    /// matching the provided <paramref name="seasonId"/> and
    /// <paramref name="tier"/>.
    /// </summary>
    /// 
    /// <param name="seasonId">
    /// The id of the required season.
    /// </param>
    /// 
    /// <param name="tier">
    /// The id of the required tier.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="seasonId"/> and
    /// <paramref name="tier"/> matched more than one competition.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="CompetitionModel"/> or
    /// null if the given <paramref name="seasonId"/> and
    /// <paramref name="tier"/> matched no competition.
    /// </returns>
    CompetitionModel? GetTierCompetition(long seasonId, int tier);
    
    /// <summary>
    /// Retrieves a model from the database for the competition
    /// matching the provided <paramref name="seasonId"/> and
    /// <paramref name="teamId"/>.
    /// </summary>
    /// 
    /// <param name="seasonId">
    /// The id of the required season.
    /// </param>
    /// 
    /// <param name="teamId">
    /// The id of the required team.
    /// </param>
    /// 
    /// <exception cref="InvalidOperationException">
    /// Thrown if the given <paramref name="seasonId"/> and
    /// <paramref name="teamId"/> matched more than one competition.
    /// </exception>
    ///
    /// <returns>
    /// Returns the matching <see cref="CompetitionModel"/> or
    /// null if the given <paramref name="seasonId"/> and
    /// <paramref name="teamId"/> matched no competition.
    /// </returns>
    CompetitionModel? GetTeamCompetition(long seasonId, long teamId);
    
    /// <summary>
    /// Retrieves models for all competitions in the season
    /// matching the given <paramref name="seasonId"/>. If no
    /// seasonId is specified, returns all competitions in the database.
    /// </summary>
    /// 
    /// <param name="seasonId">
    /// Optional. The id of the required season.
    /// </param>
    /// 
    /// <returns>
    /// A collection of <see cref="CompetitionModel">CompetitionModels</see>
    /// for each competition in the given <paramref name="seasonId"/>, or the
    /// entire database if no seasonId is provided.
    /// Can be empty if the season contained no competitions.
    /// </returns>
    CompetitionModel[] GetCompetitions(long? seasonId = null);
}

public class CompetitionRepository : ICompetitionRepository
{
    private readonly IDatabaseConnection _connection;

    public CompetitionRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public CompetitionModel? GetCompetition(long competitionId)
    {
        _connection.Open();

        var cmd = BuildCommand(competitionId);
        var competitions = GetCompetitionModels(cmd);

        _connection.Close();

        return competitions.SingleOrDefault();
    }

    public CompetitionModel? GetTierCompetition(long seasonId, int tier)
    {
        _connection.Open();

        var cmd = BuildCommand(competitionId: null, seasonId, tier);
        var competitions = GetCompetitionModels(cmd);

        _connection.Close();

        return competitions.SingleOrDefault();
    }

    public CompetitionModel? GetTeamCompetition(long seasonId, long teamId)
    {
        _connection.Open();

        var competitionId = GetCompetitionId(seasonId, teamId);

        if (competitionId is null)
        {
            _connection.Close();
            return null;
        }

        var cmd = BuildCommand(competitionId);
        var competitions = GetCompetitionModels(cmd);

        _connection.Close();

        return competitions.SingleOrDefault();
    }

    public CompetitionModel[] GetCompetitions(long? seasonId = null)
    {
        _connection.Open();

        var cmd = BuildCommand(competitionId: null, seasonId);
        var competitions = GetCompetitionModels(cmd);

        _connection.Close();

        return competitions;
    }

    private static CompetitionModel[] GetCompetitionModels(DbCommand cmd)
    {
        var competitions = new List<CompetitionModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var competitionModel = GetCompetitionModel(reader);
            competitions.Add(competitionModel);
        }

        return competitions.ToArray();
    }

    private static CompetitionModel GetCompetitionModel(DbDataReader reader)
    {
        return new(
            Id: reader.GetInt64(0),
            Name: reader.GetString(1),
            SeasonId: reader.GetInt64(2),
            StartYear: reader.GetInt16(3),
            EndYear: reader.GetInt16(4),
            Tier: reader.GetByte(5),
            Region: reader.IsDBNull(6) ? null : reader.GetString(6),
            Comment: reader.IsDBNull(7) ? null : reader.GetString(7),
            PointsForWin: reader.GetByte(8),
            TotalPlaces: reader.GetByte(9),
            PromotionPlaces: reader.GetByte(10),
            RelegationPlaces: reader.GetByte(11),
            PlayOffPlaces: reader.GetByte(12),
            RelegationPlayOffPlaces: reader.GetByte(13),
            ReElectionPlaces: reader.GetByte(14),
            FailedReElectionPosition: reader.IsDBNull(15) ? null : reader.GetByte(15)
        );
    }

    private DbCommand BuildCommand(
        long? competitionId = null,
        long? seasonId = null,
        int? tier = null)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = GetSql(competitionId, seasonId, tier);

        AddParameters(cmd, competitionId, seasonId, tier);

        return cmd;
    }

    private static void AddParameters(
        DbCommand cmd,
        long? competitionId,
        long? seasonId,
        int? tier)
    {
        if (competitionId != null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@CompetitionId",
                    Value = competitionId
                });
        }

        if (seasonId != null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@SeasonId",
                    Value = seasonId
                });
        }

        if (tier != null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Tier",
                    Value = tier
                });
        }
    }

    private static string GetSql(long? competitionId, long? seasonId, int? tier)
        => $@"
            SELECT c.Id,
                   c.Name,
                   s.Id AS SeasonId,
	               s.StartYear,
	               s.EndYear,
                   c.Tier,
                   c.Region,
                   c.Comment,
  	               cr.PointsForWin,
	               cr.TotalPlaces,
	               cr.PromotionPlaces,
	               cr.RelegationPlaces,
	               cr.PlayOffPlaces,
	               cr.RelegationPlayOffPlaces,
	               cr.ReElectionPlaces,
	               cr.FailedReElectionPosition
            FROM [dbo].[Competitions] AS c
            LEFT JOIN dbo.CompetitionRules AS cr ON cr.Id = c.RulesId
            LEFT JOIN dbo.Seasons AS s ON s.Id = c.SeasonId
            {BuildWhereClause(competitionId, seasonId, tier)}
            ";

    private static string BuildWhereClause(
        long? competitionId,
        long? seasonId,
        int? tier)
    {
        var clauses = new List<string>();

        if (competitionId != null)
        {
            clauses.Add("c.Id = @CompetitionId");
        }

        if (seasonId != null)
        {
            clauses.Add("s.Id = @SeasonId");
        }

        if (tier != null)
        {
            clauses.Add("c.Tier = @Tier");
        }

        return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : string.Empty;
    }
    
    private long? GetCompetitionId(long seasonId, long teamId)
    {
        const string sql = @"
            SELECT TOP(1) c.Id
            FROM [dbo].[Competitions] AS c
            LEFT JOIN [dbo].[Positions] AS p
                ON p.CompetitionId = c.Id
            WHERE c.SeasonId = @SeasonId AND p.TeamId = @TeamId
            ";
        
        var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        
        cmd.Parameters.Add(
            new SqlParameter
            {
                ParameterName = "@SeasonId",
                Value         = seasonId
            });
        
        cmd.Parameters.Add(
            new SqlParameter
            {
                ParameterName = "@TeamId",
                Value         = teamId
            });

        return (long?) cmd.ExecuteScalar();
    }
}