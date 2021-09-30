using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/historical-positions")]
    public class HistoricalPositionController : Controller
    {
        private readonly IHistoricalPositionBuilder _builder;

        public HistoricalPositionController(IHistoricalPositionBuilder builder)
        {
            _builder = builder;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<List<HistoricalPositionDto>> GetHistoricalPositions(long teamId, long[] seasonIds)
        {
            try
            {
                return seasonIds
                    .Select(seasonId => _builder.Build(teamId, seasonId).ToDto())
                    .ToList();
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    DataNotFoundException => NotFound(ex.Message),
                    DataInvalidException => Problem(ex.Message),
                    _ => Problem()
                };
            }
        }
    }
}