namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/teams")]
public class TeamController : Controller
{
    private readonly ITeamBuilder _builder;

    public TeamController(ITeamBuilder builder)
    {
        _builder = builder;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Team[]> GetAllTeams()
    {
        var teams = _builder.BuildAllTeams();

        if (!teams.Any())
        {
            return NotFound("No teams were found.");
        }

        return teams;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Team> GetTeam(long id)
    {
        var team = _builder.BuildTeam(id);

        if (team is null)
        {
            return NotFound($"No team was found with id {id}.");
        }
            
        return team;
    }
}