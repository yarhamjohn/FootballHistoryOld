using System.Linq;
using football.history.api.Builders;
using football.history.api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/seasons")]
public class SeasonController : Controller
{
    private readonly ISeasonBuilder _builder;

    public SeasonController(ISeasonBuilder builder)
    {
        _builder = builder;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Season[]> GetAllSeasons()
    {
        var seasons = _builder.BuildAllSeasons();

        if (!seasons.Any())
        {
            return NotFound("No seasons were found.");
        }

        return seasons;
    }

    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<Season?> GetSeason(long id)
    {
        var season = _builder.BuildSeason(id);

        if (season is null)
        {
            return NotFound($"No season was found with id {id}.");
        }
            
        return season;
    }
}