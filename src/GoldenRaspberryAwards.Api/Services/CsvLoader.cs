using GoldenRaspberryAwards.Api.Data;
using GoldenRaspberryAwards.Api.Domain.Entities;

public class CsvLoader
{
    private readonly AppDbContext _db;

    public CsvLoader(AppDbContext db) => _db = db;

    public void Load(string path)
    {
        var lines = File.ReadAllLines(path).Skip(1);

        foreach (var line in lines)
        {
            var cols = line.Split(';');

            var year = int.Parse(cols[0]);
            var title = cols[1].Trim();
            var studios = cols[2].Trim();
            var producers = cols[3];
            var winner = cols[4].Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);

            var movie = new Movie 
            { 
                Year = year, 
                Title = title,
                Studios = studios,
                Winner = winner 
            };

            var names = producers
                .Replace(" and ", ",")
                .Split(',')
                .Select(p => p.Trim());

            foreach (var name in names)
            {
                var producer = _db.Producers.FirstOrDefault(p => p.Name == name)
                               ?? new Producer { Name = name };

                movie.MovieProducers.Add(new MovieProducer
                {
                    Movie = movie,
                    Producer = producer
                });

                if (producer.Id == 0)
                    _db.Producers.Add(producer);
            }

            _db.Movies.Add(movie);
        }

        _db.SaveChanges();
    }
}
