using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Team;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/positions")]
    public class PositionController : Controller
    {
        private readonly IPositionRepository _repository;
        private readonly ICompetitionRepository _competitionRepository;

        public PositionController(IPositionRepository repository, ICompetitionRepository competitionRepository)
        {
            _repository = repository;
            _competitionRepository = competitionRepository;
        }

        [HttpGet("season/{id:long}")]
        public ApiResponse<List<PositionDto>?> GetSeasonChampions(long id)
        {
            try
            {
                var competitions = _competitionRepository.GetCompetitionsInSeason(id);

                var positions = competitions
                    .Select(competition => _repository.GetCompetitionPositions(competition.Id)
                        .Single(x => x.Position == 1)).Select(BuildPositionDto).ToList();

                return new(positions);
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

        [HttpGet("team/{id:long}")]
        public ApiResponse<List<PositionDto>?> GetTeamChampions(long id)
        {
            try
            {
                var positions = _repository.GetTeamPositions(id)
                        .Where(x => x.Position == 1).Select(BuildPositionDto).ToList();

                return new(positions);
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
        
        private static PositionDto BuildPositionDto(PositionModel position) =>
            new(position.Id, position.CompetitionId, position.CompetitionName, position.TeamId, position.TeamName, position.Position, position.Status);
    }
}