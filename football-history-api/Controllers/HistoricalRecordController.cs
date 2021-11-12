using System;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Domain;
using football.history.api.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
            var record = _builder.Build(teamId, seasonIds);

            if (!record.HistoricalSeasons.Any())
            {
                return NotFound("No historical seasons were found for the specified team " +
                                $"('{teamId}') and seasonIds ('{string.Join(",", seasonIds)}')");
            }

            return record;
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