using System.Linq;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/competitions")]
public class CompetitionController : Controller
{
    private readonly ICompetitionBuilder _builder;

    public CompetitionController(ICompetitionBuilder builder)
    {
        _builder = builder;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Competition[]> GetAllCompetitions()
    {
        var competitions = _builder.BuildCompetitions();
        
        if (!competitions.Any())
        {
            return NotFound("No competitions were found.");
        }

        return competitions;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Competition?> GetCompetition(long id)
    {
        var competition = _builder.BuildCompetition(id);

        if (competition is null)
        {
            return NotFound($"No competition was found with id {id}.");
        }
        
        return competition;
    }

    [HttpGet("season/{seasonId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Competition[]> GetCompetitions(long seasonId)
    {
        var competitions = _builder.BuildCompetitions(seasonId);
                
        if (!competitions.Any())
        {
            return NotFound("No competitions were found.");
        }

        return competitions;
    }
}