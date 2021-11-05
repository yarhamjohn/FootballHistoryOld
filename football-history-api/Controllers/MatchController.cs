using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Domain;
using football.history.api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/matches")]
    public class MatchController : Controller
    {
        private readonly IMatchBuilder _builder;

        public MatchController(IMatchBuilder matchBuilder)
        {
            _builder = matchBuilder;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Match[]> GetMatches(
            long? competitionId,
            long? seasonId,
            long? teamId,
            MatchType? type,
            DateTime? matchDate)
        {
            var matches = _builder.BuildMatches(competitionId, seasonId, teamId, type, matchDate);

            if (!matches.Any())
            {
                return NotFound("No matches were found.");
            }

            return matches;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Match> GetMatch(long id)
        {
            var match = _builder.BuildMatch(id);

            if (match is null)
            {
                return NotFound($"No match was found with id {id}.");
            }
            
            return match;
        }
    }
}