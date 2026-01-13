using GoldenRaspberryAwards.Api.Domain.Dto;

namespace GoldenRaspberryAwards.Api.Application.Commands
{
    public class ProducersIntervalResponse
    {
        public List<AwardIntervalDto> Min { get; set; } = new();
        public List<AwardIntervalDto> Max { get; set; } = new();
    }
}
