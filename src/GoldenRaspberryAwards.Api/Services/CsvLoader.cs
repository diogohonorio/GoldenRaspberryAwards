using GoldenRaspberryAwards.Api.Data;
using GoldenRaspberryAwards.Api.Domain.Entities;

public class CsvLoader
{
    private readonly AppDbContext _db;

    public CsvLoader(AppDbContext db)
    {
        _db = db;
    }

    public void Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("File path was not provided.");

        if (!File.Exists(path))
            throw new FileNotFoundException("CSV file not found.", path);

        if (!string.Equals(Path.GetExtension(path), ".csv", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The provided file is not a .csv file.");

        var fileInfo = new FileInfo(path);
        if (fileInfo.Length == 0)
            throw new InvalidOperationException("The CSV file is empty.");

        var allLines = File.ReadAllLines(path);
        if (allLines.Length <= 1)
            throw new InvalidOperationException("The CSV file does not contain any records to process.");

        var lines = allLines.Skip(1);

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
