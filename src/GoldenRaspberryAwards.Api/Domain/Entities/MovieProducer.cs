namespace GoldenRaspberryAwards.Api.Domain.Entities
{
    public class MovieProducer
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = default!;

        public int ProducerId { get; set; }
        public Producer Producer { get; set; } = default!;
    }

}
