using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace football_history.Controllers
{
    [Route("api/[controller]")]
    public class FootballHistoryController : Controller
    {
        [HttpGet("[action]")]
        public LeagueTable GetLeague()
        {
            var league = new LeagueTable 
            {
                Competition = "Premier League",
                Season = "2015 - 2016",
                LeagueTableRow = new List<LeagueTableRow> 
                {
                    new LeagueTableRow 
                    {
                        Position = 1,
                        Team = "Team 1",
                        Played = 38,
                        Won = 28,
                        Drawn = 4,
                        Lost = 6,
                        GoalsFor = 100,
                        GoalsAgainst = 50,
                        GoalDifference = 50,
                        Points = 88
                    },
                    new LeagueTableRow 
                    {
                        Position = 2,
                        Team = "Team 2",
                        Played = 38,
                        Won = 28,
                        Drawn = 3,
                        Lost = 7,
                        GoalsFor = 100,
                        GoalsAgainst = 50,
                        GoalDifference = 50,
                        Points = 87
                    }
                }
            };

            return league;
        }
    }
}
