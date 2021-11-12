using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Models;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories;

public interface IMatchRepository
{
    public MatchModel[] GetMatches(
        long? competitionId = null,
        long? seasonId = null,
        long? teamId = null,
        MatchType? type = null,
        DateTime? matchDate = null);

    public MatchModel? GetMatch(long matchId);

    public MatchModel[] GetLeagueMatches(long competitionId);

    public MatchModel[] GetPlayOffMatches(long competitionId);
}

public class MatchRepository : IMatchRepository
{
    private readonly IDatabaseConnection _connection;

    public MatchRepository(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public MatchModel[] GetMatches(
        long? competitionId = null,
        long? seasonId = null,
        long? teamId = null,
        MatchType? type = null,
        DateTime? matchDate = null)
    {
        _connection.Open();

        var cmd = BuildCommand(_connection, matchId: null, competitionId, seasonId, teamId, type, matchDate);
        var matches = GetMatchModels(cmd);

        _connection.Close();

        return matches;
    }

    public MatchModel? GetMatch(long matchId)
    {
        _connection.Open();

        var cmd = BuildCommand(_connection, matchId);
        var matches = GetMatchModels(cmd);

        _connection.Close();

        return matches.SingleOrDefault();
    }

    public MatchModel[] GetLeagueMatches(long competitionId)
        => GetMatches(competitionId, seasonId: null, teamId: null, MatchType.League);

    public MatchModel[] GetPlayOffMatches(long competitionId)
        => GetMatches(competitionId, seasonId: null, teamId: null, MatchType.PlayOff);

    private static MatchModel[] GetMatchModels(DbCommand cmd)
    {
        var matches = new List<MatchModel>();

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            matches.Add(GetMatchModel(reader));
        }

        return matches.ToArray();
    }

    private static MatchModel GetMatchModel(DbDataReader reader)
        => new(
            Id: reader.GetInt64(0),
            MatchDate: reader.GetDateTime(1),
            CompetitionId: reader.GetInt64(2),
            CompetitionName: reader.GetString(3),
            CompetitionStartYear: reader.GetInt16(4),
            CompetitionEndYear: reader.GetInt16(5),
            CompetitionTier: reader.GetByte(6),
            CompetitionRegion: reader.IsDBNull(7) ? null : reader.GetString(7),
            RulesType: reader.GetString(8),
            RulesStage: reader.IsDBNull(9) ? null : reader.GetString(9),
            RulesExtraTime: reader.GetBoolean(10),
            RulesPenalties: reader.GetBoolean(11),
            RulesNumLegs: reader.IsDBNull(12) ? null : reader.GetByte(12),
            RulesAwayGoals: reader.GetBoolean(13),
            RulesReplays: reader.GetBoolean(14),
            HomeTeamId: reader.GetInt64(15),
            HomeTeamName: reader.GetString(16),
            HomeTeamAbbreviation: reader.GetString(17),
            AwayTeamId: reader.GetInt64(18),
            AwayTeamName: reader.GetString(19),
            AwayTeamAbbreviation: reader.GetString(20),
            HomeGoals: reader.GetByte(21),
            AwayGoals: reader.GetByte(22),
            HomeGoalsExtraTime: reader.GetByte(23),
            AwayGoalsExtraTime: reader.GetByte(24),
            HomePenaltiesTaken: reader.GetByte(25),
            HomePenaltiesScored: reader.GetByte(26),
            AwayPenaltiesTaken: reader.GetByte(27),
            AwayPenaltiesScored: reader.GetByte(28));
        
    private static DbCommand BuildCommand(
        IDatabaseConnection connection,
        long? matchId = null,
        long? competitionId = null, 
        long? seasonId = null, 
        long? teamId = null, 
        MatchType? type = null,
        DateTime? matchDate = null)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = GetSql(matchId, competitionId, seasonId, teamId, type, matchDate);

        AddParameters(cmd, matchId, competitionId, seasonId, teamId, type, matchDate);
            
        return cmd;
    }
        
    private static void AddParameters(
        DbCommand cmd,
        long? matchId,
        long? competitionId, 
        long? seasonId, 
        long? teamId, 
        MatchType? type,
        DateTime? matchDate)
    {
        if (matchId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@MatchId",
                    Value         = matchId
                });
        } 
            
        if (competitionId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@CompetitionId",
                    Value         = competitionId
                });
        }

        if (seasonId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@SeasonId",
                    Value         = seasonId
                });
        }

        if (teamId is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@HomeTeamId",
                    Value         = teamId
                });

            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@AwayTeamId",
                    Value         = teamId
                });
        }

        if (type is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Type",
                    Value         = Enum.GetName((MatchType) type)
                });
        }

        if (matchDate is not null)
        {
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@MatchDate",
                    Value         = matchDate
                });
        }
    }

    private static string GetSql(
        long? matchId,
        long? competitionId, 
        long? seasonId, 
        long? teamId, 
        MatchType? type,
        DateTime? matchDate)
        => $@"
                SELECT m.Id
                      ,m.MatchDate
                      ,c.Id AS CompetitionId
                      ,c.Name AS CompetitionName
                      ,s.StartYear
                      ,s.EndYear
                      ,c.Tier
                      ,c.Region
                      ,r.Type
                      ,r.Stage
                      ,r.ExtraTime
                      ,r.Penalties
                      ,r.NumLegs
                      ,r.AwayGoals
                      ,r.Replays
                      ,ht.Id AS HomeTeamId
                      ,ht.Name As HomeTeamName
                      ,ht.Abbreviation AS HomeTeamAbbreviation
                      ,at.Id AS AwayTeamId
                      ,at.Name AS AwayTeamName
                      ,at.Abbreviation AS AwayTeamAbbreviation
                      ,m.HomeGoals
                      ,m.AwayGoals
                      ,m.HomeGoalsET
                      ,m.AwayGoalsET
                      ,m.HomePenaltiesTaken
                      ,m.HomePenaltiesScored
                      ,m.AwayPenaltiesTaken
                      ,m.AwayPenaltiesScored
                FROM [dbo].[Matches] AS m
                LEFT JOIN [dbo].[Competitions] AS c ON c.Id = m.CompetitionId
                LEFT JOIN [dbo].[MatchRules] AS r ON r.Id = m.RulesId
                LEFT JOIN [dbo].[Teams] AS ht ON ht.Id = m.HomeTeamId
                LEFT JOIN [dbo].[Teams] AS at ON at.Id = m.AwayTeamId
                LEFT JOIN [dbo].[Seasons] AS s ON s.Id = c.SeasonId
                {BuildWhereClause(matchId, competitionId, seasonId, teamId, type, matchDate)}
                ";
        
    private static string BuildWhereClause(
        long? matchId,
        long? competitionId, 
        long? seasonId, 
        long? teamId, 
        MatchType? type, 
        DateTime? matchDate)
    {
        var clauses = new List<string>();

        if (matchId is not null)
        {
            clauses.Add("m.Id = @MatchId");
        }

        if (competitionId is not null)
        {
            clauses.Add("c.Id = @CompetitionId");
        }

        if (seasonId is not null)
        {
            clauses.Add("s.Id = @SeasonId");
        }

        if (teamId is not null)
        {
            clauses.Add("(ht.Id = @HomeTeamId OR at.Id = @AwayTeamId)");
        }

        if (type is not null)
        {
            clauses.Add("r.Type = @Type");
        }

        if (matchDate is not null)
        {
            clauses.Add("m.MatchDate = @MatchDate");
        }

        return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : string.Empty;
    }
}