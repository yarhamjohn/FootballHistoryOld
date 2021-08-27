using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Builders.Statistics;
using football.history.api.Builders.Team;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.Season;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsBuilder _statisticsBuilder;

        public StatisticsController(IStatisticsBuilder statisticsBuilder)
        {
            _statisticsBuilder = statisticsBuilder;
        }

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("api/v{version:apiVersion}/statistics/season/{id:long}")]
        public ApiResponse<List<StatisticDto>?> GetSeasonStatistics(long id)
        {
            try
            {
                var statistics = _statisticsBuilder.BuildSeasonStatistics(id);
                return new(statistics);
            }
            catch (FootballHistoryException ex)
            {
                return new(
                    Result: null,
                    Error: new(ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                return new(
                    Result: null,
                    Error: new($"Something went wrong. {ex.Message}"));
            }
        }
    }
}
