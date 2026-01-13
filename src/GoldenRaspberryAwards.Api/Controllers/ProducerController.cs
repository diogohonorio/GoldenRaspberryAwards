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

        public ProducerController(GetProducersIntervalsHandler handler)
        {
            _handler = handler;
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}

    
