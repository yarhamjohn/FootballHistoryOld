using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace football_history.Server.Repositories
{
    public class FootballHistoryRepository : IFootballHistoryRepository
    {
        private FootballHistoryContext m_Context { get; }

        public FootballHistoryRepository(FootballHistoryContext context)
        {
            m_Context = context;
        }

        public LeagueTable GetLeagueTable(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);
            var leagueTable = new LeagueTable
            {
                Season = $"{seasonStartYear} - {seasonEndYear}",
                LeagueTableRows = new List<LeagueTableRow>()
            };

            var matchesInSeason = @"
SELECT hc.Name AS HomeTeam
    ,ac.Name AS AwayTeam
    ,m.HomeGoals
    ,m.AwayGoals
    ,m.DivisionId
    ,d.Name AS Division
FROM dbo.LeagueMatches m
INNER JOIN dbo.Divisions d ON m.DivisionId = d.Id
INNER JOIN dbo.Clubs hc ON m.HomeClubId = hc.Id
INNER JOIN dbo.Clubs ac ON m.AwayClubId = ac.Id
WHERE d.Tier = @Tier
    AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var sql = $@"
WITH MatchesInSeason AS ({matchesInSeason})

SELECT Position
    ,Division
    ,Team
    ,GamesPlayed
    ,GamesWon
    ,GamesDrawn
    ,GamesLost
    ,GoalsFor
    ,GoalsAgainst
    ,GoalDifference
    ,Points
    ,PointsDeducted
    ,PointsDeductionReason
    ,Status
FROM (
    SELECT ROW_NUMBER() OVER(ORDER BY Points DESC, GoalDifference DESC, GoalsFor DESC) AS Position
        ,Division
        ,Team
        ,GamesPlayed
        ,GamesWon
        ,GamesDrawn
        ,GamesLost
        ,GoalsFor
        ,GoalsAgainst
        ,GoalDifference
        ,Points
        ,PointsDeducted
        ,PointsDeductionReason
    FROM (
        SELECT Division
            ,Team
            ,HomeWins + HomeDraws + HomeLosses + AwayWins + AwayDraws + AwayLosses AS GamesPlayed
            ,HomeWins + AwayWins AS GamesWon
            ,HomeDraws + AwayDraws AS GamesDrawn
            ,HomeLosses + AwayLosses AS GamesLost
            ,HomeGoalsFor + AwayGoalsFor AS GoalsFor
            ,HomeGoalsAgainst + AwayGoalsAgainst AS GoalsAgainst
            ,HomeGoalsFor + AwayGoalsFor - HomeGoalsAgainst - AwayGoalsAgainst AS GoalDifference
            ,(HomeWins + AwayWins) * 3 + HomeDraws + AwayDraws - PointsDeducted AS Points
            ,PointsDeducted
            ,PointsDeductionReason
        FROM (
            SELECT m.HomeTeam AS Team
                ,m.Division
                ,COALESCE(pd.PointsDeducted, 0) AS PointsDeducted
                ,pd.Reason AS PointsDeductionReason
            FROM (
                SELECT HomeTeam, Division, DivisionId
                FROM MatchesInSeason
                GROUP BY HomeTeam, Division, DivisionId
            ) m
            LEFT JOIN (
                SELECT DivisionId, Season, Name AS Club, PointsDeducted, Reason
                FROM dbo.PointDeductions pd
                INNER JOIN dbo.Clubs c ON pd.ClubId = c.Id
            ) pd
            ON m.DivisionId = pd.DivisionId
                AND m.HomeTeam = pd.Club
                AND pd.Season = CONCAT(@SeasonStartYear, ' - ', @SeasonEndYear)
        ) t
        INNER JOIN (
                SELECT HomeTeam
                    ,SUM(CASE WHEN HomeGoals > AwayGoals THEN 1 ELSE 0 END) AS HomeWins
                    ,SUM(CASE WHEN HomeGoals = AwayGoals THEN 1 ELSE 0 END) AS HomeDraws 
                    ,SUM(CASE WHEN HomeGoals < AwayGoals THEN 1 ELSE 0 END) AS HomeLosses
                    ,SUM(HomeGoals) AS HomeGoalsFor
                    ,SUM(AwayGoals) AS HomeGoalsAgainst
                FROM MatchesInSeason
                GROUP BY HomeTeam
            ) AS m1 ON t.Team = m1.HomeTeam
        INNER JOIN (
                SELECT AwayTeam
                    ,SUM(CASE WHEN AwayGoals > HomeGoals THEN 1 ELSE 0 END) AS AwayWins
                    ,SUM(CASE WHEN AwayGoals = HomeGoals THEN 1 ELSE 0 END) AS AwayDraws
                    ,SUM(CASE WHEN AwayGoals < HomeGoals THEN 1 ELSE 0 END) AS AwayLosses
                    ,SUM(AwayGoals) AS AwayGoalsFor
                    ,SUM(HomeGoals) AS AwayGoalsAgainst
                FROM MatchesInSeason
                GROUP BY AwayTeam
            ) AS m2 ON t.Team = m2.AwayTeam
        ) r
) a
CROSS APPLY (
    SELECT CASE WHEN a.Position = 1 THEN 'Champions' 
                WHEN a.Position <= ls.PromotionPlaces THEN 'Promoted' 
                WHEN a.Team = pom.PlayOffWinner THEN 'Play Off Winner'
                WHEN a.Position <= ls.PromotionPlaces + ls.PlayOffPlaces THEN 'Play Offs'
                WHEN a.Position > ls.TotalPlaces - ls.RelegationPlaces THEN 'Relegated'
                ELSE NULL END AS Status
    FROM dbo.LeagueStatuses ls
    INNER JOIN dbo.Divisions d ON d.Id = ls.DivisionId
    LEFT JOIN (
        SELECT DivisionId
            ,MatchDate
            ,CASE WHEN (homegoals + homegoalset) > (awaygoals + awaygoalset) THEN hc.name 
                WHEN (homegoals + homegoalset) < (awaygoals + awaygoalset) THEN ac.name
	            ELSE CASE WHEN homepenaltiesscored > awaypenaltiesscored THEN hc.name 
                    ELSE ac.name END
                END AS PlayOffWinner 
        FROM dbo.PlayOffMatches pom
        LEFT JOIN dbo.Clubs hc ON hc.Id = pom.HomeClubId
        LEFT JOIN dbo.Clubs ac ON ac.Id = pom.AwayClubId
        WHERE pom.Round = 'Final'
            AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
    ) pom
    ON pom.DivisionId = ls.DivisionId

    WHERE d.Tier = @Tier AND ls.Season = CONCAT(@SeasonStartYear, ' - ', @SeasonEndYear)
) b
ORDER BY Position;
";

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Tier", tier));
                cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
                cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var position = (int) reader.GetInt64(0);

                        leagueTable.Competition = reader.GetString(1);
                        leagueTable.LeagueTableRows.Add(
                            new LeagueTableRow
                            {
                                Position = position,
                                Team = reader.GetString(2),
                                Played = reader.GetInt32(3),
                                Won = reader.GetInt32(4),
                                Drawn = reader.GetInt32(5),
                                Lost = reader.GetInt32(6),
                                GoalsFor = reader.GetInt32(7),
                                GoalsAgainst = reader.GetInt32(8),
                                GoalDifference = reader.GetInt32(9),
                                Points = reader.GetInt32(10),
                                PointsDeducted = reader.GetInt32(11),
                                PointsDeductionReason = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Status = reader.IsDBNull(13) ? string.Empty : reader.GetString(13)
                            }
                        );
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return leagueTable;
        }

        public LeagueFilterOptions GetLeagueFilterOptions()
        {
            var leagueFilterOptions = new LeagueFilterOptions
            {
                AllSeasons = new List<string>(),
                AllTiers = new List<Tier>()
            };

            var sql = @"
SELECT Season
FROM (
    SELECT CASE WHEN MONTH(MatchDate) >= 7 
                THEN CAST(YEAR(MatchDate) AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) + 1 AS CHAR(4))
                ELSE CAST(YEAR(MatchDate) - 1 AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) AS CHAR(4))
                END AS Season
    FROM dbo.LeagueMatches
) m
GROUP BY Season;

SELECT Name, Tier, [From], [To]
FROM dbo.Divisions;
";

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        leagueFilterOptions.AllSeasons.Add(
                            reader.GetString(0)
                        );
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        var tier = reader.GetByte(1);
                        var division = new Division
                            {
                                Name = reader.GetString(0),
                                SeasonStartYear = reader.GetInt16(2),
                                SeasonEndYear = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                            };

                        AddDivision(tier, division, leagueFilterOptions);
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return leagueFilterOptions;
        }

        public List<Results> GetMatchResultMatrix(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);
            
            var sql = @"
SELECT hc.Name AS HomeTeam
    ,ac.Name AS AwayTeam
    ,hc.Abbreviation AS HomeTeamAbbreviation
    ,CONCAT(m.HomeGoals, '-', AwayGoals) AS Score
    ,CASE WHEN m.HomeGoals > m.AwayGoals THEN 'W'
          WHEN m.HomeGoals = m.AwayGoals THEN 'D'
          ELSE 'L' END AS Result
FROM dbo.LeagueMatches m
INNER JOIN dbo.Divisions d ON m.DivisionId = d.Id
INNER JOIN dbo.Clubs hc ON m.HomeClubId = hc.Id
INNER JOIN dbo.Clubs ac ON m.AwayClubId = ac.Id
WHERE d.Tier = @Tier
    AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var resultsMatrix = new List<Results>();

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Tier", tier));
                cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
                cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var homeTeam = reader.GetString(0);
                        var awayTeam = reader.GetString(1);
                        var homeTeamAbbreviation = reader.GetString(2);
                        var score = reader.GetString(3);
                        var result = reader.GetString(4);

                        AddResult(homeTeam, awayTeam, homeTeamAbbreviation, score, result, resultsMatrix);
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }
        
            return resultsMatrix;
        }

        public PlayOff GetPlayOffMatches(int tier, string season)
        {
            if (tier == 1) {
                return new PlayOff();
            }

            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);
            
            var sql = @"
SELECT Round
    ,hc.Name AS HomeClub
    ,ac.Name AS AwayClub
    ,HomeGoals
    ,AwayGoals
    ,ExtraTime
    ,HomeGoalsET
    ,AwayGoalsET
    ,PenaltyShootout
    ,HomePenaltiesTaken
    ,HomePenaltiesScored
    ,AwayPenaltiesTaken
    ,AwayPenaltiesScored
    ,MatchDate
FROM dbo.PlayOffMatches pom
INNER JOIN dbo.Divisions d ON pom.DivisionId = d.Id
INNER JOIN dbo.Clubs hc ON hc.Id = pom.HomeClubId
INNER JOIN dbo.Clubs ac ON ac.Id = pom.AwayClubId
WHERE d.Tier = @Tier
    AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
ORDER BY MatchDate
";

            var playoffStages = new PlayOff
            {
                SemiFinals = new List<PlayOffSemiFinal>
                {
                    new PlayOffSemiFinal
                    {
                        FirstLeg = new PlayOffResult(),
                        SecondLeg = new PlayOffResult()
                    },
                    new PlayOffSemiFinal
                    {
                        FirstLeg = new PlayOffResult(),
                        SecondLeg = new PlayOffResult()
                    }
                },
                Final = new PlayOffResult()
            };

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Tier", tier));
                cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
                cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var count = 0;

                    while (reader.Read())
                    {
                        var playOffResult = GetPlayOffResult(reader);
                        if (reader.GetString(0) == "Final")
                        {
                            playoffStages.Final = playOffResult;
                        }
                        else
                        {
                            if (count == 0)
                            {
                                Console.WriteLine(playOffResult);
                                Console.WriteLine(playoffStages.SemiFinals[0].FirstLeg);

                                playoffStages.SemiFinals[0].FirstLeg = playOffResult;

                                Console.WriteLine(playoffStages.SemiFinals[0].FirstLeg);

                            }
                            else if (count == 1)
                            {
                                playoffStages.SemiFinals[1].FirstLeg = playOffResult;
                            }
                            else if (playOffResult.HomeTeam == playoffStages.SemiFinals[0].FirstLeg.AwayTeam)
                            {
                                playoffStages.SemiFinals[0].SecondLeg = playOffResult;
                            }
                            else
                            {
                                playoffStages.SemiFinals[1].SecondLeg = playOffResult;
                            }

                            count++;
                        }
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }
        
            return playoffStages;
        }

        public List<Match> GetLeagueTableDrillDown(int tier, string season, string team) 
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);
            
            var sql = @"
SELECT MatchDate
    ,CASE WHEN hc.Name = @Team AND HomeGoals > AwayGoals THEN 'W' 
          WHEN hc.Name = @Team AND HomeGoals < AwayGoals THEN 'L'
          WHEN ac.Name = @Team AND HomeGoals > AwayGoals THEN 'L'
          WHEN ac.Name = @Team AND HomeGoals < AwayGoals THEN 'W'
          ELSE 'D' END AS Result
FROM dbo.LeagueMatches m
INNER JOIN dbo.Divisions d ON m.DivisionId = d.Id
INNER JOIN dbo.Clubs hc ON hc.Id = m.HomeClubId
INNER JOIN dbo.Clubs ac ON ac.Id = m.AwayClubId
WHERE d.Tier = @Tier
    AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
    AND (hc.Name = @Team OR ac.Name = @Team)
ORDER BY MatchDate
";

            var matches = new List<Match>();

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Tier", tier));
                cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
                cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));
                cmd.Parameters.Add(new SqlParameter("@Team", team));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        matches.Add(
                            new Match {
                                MatchDate = reader.GetDateTime(0),
                                Result = reader.GetString(1),
                            }
                        );
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }
        
            return matches;
        }

        private PlayOffResult GetPlayOffResult(DbDataReader reader)
        {
            return new PlayOffResult
            {
                HomeTeam = reader.GetString(1),
                AwayTeam = reader.GetString(2),
                HomeGoals = reader.GetByte(3),
                AwayGoals = reader.GetByte(4),
                HomeGoalsET = reader.GetBoolean(5) ? reader.GetByte(6) : (int?) null,
                AwayGoalsET = reader.GetBoolean(5) ? reader.GetByte(7) : (int?) null,
                HomePenaltiesTaken = reader.GetBoolean(8) ? reader.GetByte(9) : (int?) null,
                HomePenaltiesScored = reader.GetBoolean(8) ? reader.GetByte(10) : (int?) null,
                AwayPenaltiesTaken = reader.GetBoolean(8) ? reader.GetByte(11) : (int?) null,
                AwayPenaltiesScored = reader.GetBoolean(8) ? reader.GetByte(12) : (int?) null
            };
        }

        private void AddDivision(int tier, Division division, LeagueFilterOptions leagueFilterOptions)
        {
            var tierExists = leagueFilterOptions.AllTiers.Where(t => t.Level == tier).ToList().Count == 1;
            if (tierExists)
            {
                leagueFilterOptions.AllTiers = leagueFilterOptions.AllTiers
                    .Select(t => {
                        if (t.Level == tier) {
                            t.Divisions.Add(division);
                        }; 
                        return t;
                    }).ToList();
            }
            else 
            {
                leagueFilterOptions.AllTiers.Add(
                    new Tier
                    {
                        Level = tier,
                        Divisions = new List<Division> { division }
                    }
                );
            }
        }

        private void AddResult(string homeTeam, string awayTeam, string homeTeamAbbreviation, string score, string result, List<Results> resultsMatrix)
        {
            var homeTeamExists = resultsMatrix.Where(r => r.HomeTeam == homeTeam).ToList().Count == 1;
            if (homeTeamExists)
            {
                resultsMatrix = resultsMatrix
                    .Select(r => {
                        if (r.HomeTeam == homeTeam) {
                            r.Scores.Add((awayTeam,  score, result));
                        }; 
                        return r;
                    }).ToList();
            }
            else 
            {
                resultsMatrix.Add(
                    new Results
                    {
                        HomeTeam = homeTeam,
                        HomeTeamAbbreviation = homeTeamAbbreviation,
                        Scores = new List<(string AwayTeam,string Score, string Result)> { (homeTeam, null, null), (awayTeam, score, result) }
                    }
                );
            }
        }
    }
}