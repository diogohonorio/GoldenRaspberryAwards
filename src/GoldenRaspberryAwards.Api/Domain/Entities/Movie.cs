namespace GoldenRaspberryAwards.Api.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public bool Winner { get; set; }

        public ICollection<MovieProducer> MovieProducers { get; set; } = new List<MovieProducer>();
    }
}
