using GoldenRaspberryAwards.Api.Application.Commands;
using GoldenRaspberryAwards.Api.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace GoldenRaspberryAwards.Api.Controllers
{
    [ApiController]
    [Route("api/producers")]
    public class ProducerController : ControllerBase
    {
        private readonly GetProducersIntervalsHandler _handler;
        private readonly ILogger<ProducerController> _logger;

        public ProducerController(GetProducersIntervalsHandler handler, ILogger<ProducerController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        [HttpGet("intervals")]
        [ProducesResponseType(typeof(ProducersIntervalResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _handler.HandleAsync(cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GET /api/producers/intervals");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while processing the request." });
            }
        }
    }
}

    
