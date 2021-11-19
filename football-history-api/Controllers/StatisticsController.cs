namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/statistics")]
public class StatisticsController : Controller
{
    private readonly IStatisticsBuilder _statisticsBuilder;

    public StatisticsController(IStatisticsBuilder statisticsBuilder)
    {
        _statisticsBuilder = statisticsBuilder;
    }

    [HttpGet]
    [Route("season/{seasonId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<CategorisedStatistics[]> GetSeasonStatistics(long seasonId)
    {
        var statistics = _statisticsBuilder.BuildSeasonStatistics(seasonId);

        if (!statistics.Any())
        {
            return NotFound($"No statistics available for the specified season ({seasonId}).");
        }
        
        return statistics;
    }
}