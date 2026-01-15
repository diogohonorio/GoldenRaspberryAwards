using GoldenRaspberryAwards.Api.Application.Commands;
using GoldenRaspberryAwards.Api.Services;

namespace GoldenRaspberryAwards.Api.Application.Handlers;

public class GetProducersIntervalsHandler
{
    private readonly IntervalService _service;

    public GetProducersIntervalsHandler(IntervalService service)
    {
        _service = service;
    }

    public Task<ProducersIntervalResponse> HandleAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_service.Calculate());
    }
}
