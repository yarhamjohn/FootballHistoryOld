using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.DivisionRepository;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public class TierRepository : ITierRepository
    {
        private TierRepositoryContext Context { get; }

        public TierRepository(TierRepositoryContext context)
        {
            Context = context;
        }
        
        public SeasonTierFilter[] GetSeasonTierFilters(string team, int seasonStartYear, int seasonEndYear)
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, team);
                return GetTierBySeason(cmd, seasonStartYear, seasonEndYear);
            }
        }
        
        private static SeasonTierFilter[] GetTierBySeason(DbCommand cmd, int seasonStartYear, int seasonEndYear)
        {
            var result = new List<SeasonTierFilter>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        new SeasonTierFilter
                        {
                            Tier = reader.GetByte(0),
                            Season = reader.GetString(1)
                        }
                    );
                }
            }
            
            for (var year = seasonStartYear; year <= seasonEndYear; year++)
            {
                if (result.All(r => r.Season.Substring(0, 4) != year.ToString()))
                {
                    result.Add(
                        new SeasonTierFilter
                        {
                            Tier = 0,
                            Season = $"{year} - {year + 1}"
                        }
                    );
                }
            }

            return result.ToArray();
        }

        private static DbCommand GetDbCommand(DbConnection conn, string team)
        {
            const string sql = @"
SELECT tier, season
FROM (
    SELECT d.Tier
      ,CASE WHEN MONTH(lm.MatchDate) >= 7 
            THEN CONCAT(YEAR(lm.MatchDate), ' - ', (YEAR(lm.MatchDate) + 1)) 
            ELSE CONCAT((YEAR(lm.MatchDate) - 1), ' - ', YEAR(lm.MatchDate)) 
            END AS Season
    FROM [dbo].[LeagueMatches] lm
    INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
    INNER JOIN dbo.Clubs c ON c.Id = lm.HomeClubId
    WHERE c.Name = @TeamName
) AS a
GROUP BY a.Tier, a.Season
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@TeamName", team));

            return cmd;
        }
    }
}