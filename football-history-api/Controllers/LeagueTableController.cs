using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/league-table")]
public class LeagueTableController : Controller
{
    private readonly ILeagueTableBuilder _leagueTableBuilder;

    public LeagueTableController(ILeagueTableBuilder leagueTableBuilder)
    {
        _leagueTableBuilder = leagueTableBuilder;
    }

    [HttpGet("competition/{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<LeagueTable> GetLeagueTable(long id)
    {
            var leagueTable = _leagueTableBuilder.BuildFullLeagueTable(id);

            if (leagueTable is null)
            {
                return NotFound($"No league table available for competition {id}.");
            }
            
            return leagueTable;
    }

    [HttpGet("season/{seasonId:long}/team/{teamId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<LeagueTable> GetLeagueTable(long seasonId, long teamId)
    {
        var leagueTable = _leagueTableBuilder.BuildFullLeagueTable(seasonId, teamId);

        if (leagueTable is null)
        {
            return NotFound($"No league table available for season {seasonId} and team {teamId}.");
        }
            
        return leagueTable;
    }
}