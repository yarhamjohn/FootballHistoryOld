using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders.Match;
using football.history.api.Exceptions;
using football.history.api.Repositories.Match;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/matches")]
    public class MatchController : Controller
    {
        private readonly IMatchRepository _repository;

        public MatchController(IMatchRepository matchRepository)
        {
            _repository = matchRepository;
        }

        [HttpGet]
        public IActionResult GetMatches(
            long? competitionId,
            long? seasonId,
            long? teamId,
            MatchType? type,
            DateTime? matchDate)
        {
            try
            {
                var matches = _repository
                    .GetMatches(competitionId, seasonId, teamId, type, matchDate)
                    .Select(BuildMatchDto)
                    .ToList();
                return Ok(matches);
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

        [HttpGet("{id:long}")]
        public IActionResult GetMatch(long id)
        {
            try
            {
                var match = _repository.GetMatch(id);
                return Ok(BuildMatchDto(match));
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

        private static MatchDto BuildMatchDto(MatchModel match) =>
            new(match.Id,
                match.MatchDate,
                Competition: new(
                    match.CompetitionId,
                    match.CompetitionName,
                    match.CompetitionStartYear,
                    match.CompetitionEndYear,
                    match.CompetitionLevel),
                Rules: new(
                    match.RulesType,
                    match.RulesStage,
                    match.RulesExtraTime,
                    match.RulesPenalties,
                    match.RulesNumLegs,
                    match.RulesAwayGoals,
                    match.RulesReplays),
                HomeTeam: new(
                    match.HomeTeamId,
                    match.HomeTeamName,
                    match.HomeTeamAbbreviation,
                    match.HomeGoals,
                    match.HomeGoalsExtraTime,
                    match.HomePenaltiesTaken,
                    match.HomePenaltiesScored),
                AwayTeam: new(
                    match.AwayTeamId,
                    match.AwayTeamName,
                    match.AwayTeamAbbreviation,
                    match.AwayGoals,
                    match.AwayGoalsExtraTime,
                    match.AwayPenaltiesTaken,
                    match.AwayPenaltiesScored));
    }
}