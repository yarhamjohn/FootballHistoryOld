namespace football.history.api.Controllers;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/historical-record")]
public class HistoricalRecordController : Controller
{
    private readonly IHistoricalRecordBuilder _builder;

    public HistoricalRecordController(IHistoricalRecordBuilder builder)
    {
        _builder = builder;
    }

    [HttpGet("teamId/{teamId:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult<HistoricalRecord> GetHistoricalRecord(long teamId, long[] seasonIds)
    {
        try
        {
            return _builder.Build(teamId, seasonIds);
        }
        catch (Exception ex)
        {
            return ex switch
            {
                DataInvalidException => Problem(ex.Message, null, null, "Invalid data was found."),
                _ => Problem(ex.Message)
            };
        }
    }
}