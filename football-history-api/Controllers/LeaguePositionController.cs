using System;
using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/league-positions")]
    public class LeaguePositionController : Controller
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ILeaguePositionBuilder _leaguePositionBuilder;

        public LeaguePositionController(
            ICompetitionRepository competitionRepository,
            ILeaguePositionBuilder leaguePositionBuilder)
        {
            _competitionRepository = competitionRepository;
            _leaguePositionBuilder = leaguePositionBuilder;
        }

        [HttpGet]
        public IActionResult GetLeaguePositions(long teamId, long competitionId)
        {
            try
            {
                var competition = _competitionRepository.GetCompetition(competitionId);
                var leaguePositions = _leaguePositionBuilder.GetPositions(teamId, competition);

                return Ok(leaguePositions);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    DataNotFoundException => NotFound(ex.Message),
                    DataInvalidException => Problem(ex.Message),
                    _ => Problem()
                };
            }
        }
    }
}