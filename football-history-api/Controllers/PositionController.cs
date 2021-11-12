using System.Linq;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/positions")]
public class PositionController : Controller
{
    private readonly IPositionBuilder _builder;

    public PositionController(IPositionBuilder builder)
    {
        _builder = builder;
    }

    [HttpGet("season/{seasonId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Position[]> GetSeasonPositions(long seasonId)
    {
        var positions = _builder.BuildSeasonPositions(seasonId);
        
        if (!positions.Any())
        {
            return NotFound($"No positions were found for the given season ({seasonId}).");
        }

        return positions;
    }

    [HttpGet("team/{teamId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Position[]> GetTeamPositions(long teamId)
    {
        var positions = _builder.BuildTeamPositions(teamId);

        if (!positions.Any())
        {
            return NotFound($"No positions were found for the given team ({teamId}).");
        }

        return positions;
    }
}