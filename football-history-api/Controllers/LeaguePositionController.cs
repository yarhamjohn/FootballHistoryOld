namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/league-positions")]
public class LeaguePositionController : Controller
{
    private readonly ILeaguePositionBuilder _leaguePositionBuilder;

    public LeaguePositionController(ILeaguePositionBuilder leaguePositionBuilder)
    {
        _leaguePositionBuilder = leaguePositionBuilder;
    }

    [HttpGet]
    [Route("competition/{competitionId:long}/team/{teamId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<LeaguePosition[]> GetLeaguePositions(long competitionId, long teamId)
    {
        var positions = _leaguePositionBuilder.GetPositions(competitionId, teamId);

        if (!positions.Any())
        {
            return NotFound($"No league positions found for teamId {teamId} and competitionId {competitionId}.");
        }

        return positions;
    }
}