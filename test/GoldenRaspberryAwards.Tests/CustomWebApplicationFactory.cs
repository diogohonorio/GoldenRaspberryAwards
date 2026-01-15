using GoldenRaspberryAwards.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Add in-memory database for tests
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var loader = scope.ServiceProvider.GetRequiredService<CsvLoader>();
            string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "movielist.csv");

            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV file path was not provided.");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("CSV file not found.", csvPath);

            if (!string.Equals(Path.GetExtension(csvPath), ".csv", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The provided file is not a .csv file.");

            var fileInfo = new FileInfo(csvPath);
            if (fileInfo.Length == 0)
                throw new InvalidOperationException("The CSV file is empty.");

            var allLines = File.ReadAllLines(csvPath);
            if (allLines.Length <= 1)
                throw new InvalidOperationException("The CSV file does not contain any records to process.");

            loader.Load(csvPath);
        });
    }
}
