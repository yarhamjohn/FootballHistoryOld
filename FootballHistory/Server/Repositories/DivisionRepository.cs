using System;
using System.Collections.Generic;
using System.Data.Common;
using FootballHistory.Server.Domain;
using FootballHistory.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Server.Repositories
{
    public class DivisionRepository : IDivisionRepository
    {
        private LeagueSeasonContext Context { get; }

        public DivisionRepository(LeagueSeasonContext context)
        {
            Context = context;
        }
        
        public List<DivisionModel> GetDivisionModels()
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn);
                return GetDivisions(cmd);
            }
        }

        private static List<DivisionModel> GetDivisions(DbCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                var divisionModels = new List<DivisionModel>();
                while (reader.Read())
                {
                    divisionModels.Add(
                        new DivisionModel
                        {
                            Name = reader.GetString(0),
                            Tier = reader.GetByte(1),
                            From = reader.GetInt16(2),
                            To = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                        }
                    );
                }

                return divisionModels;
            }
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"
SELECT [Name]
      ,[Tier]
      ,[From]
      ,[To]
  FROM [dbo].[Divisions]
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            return cmd;
        }
    }
}
