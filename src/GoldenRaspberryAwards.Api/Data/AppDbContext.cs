namespace GoldenRaspberryAwards.Api.Data
{
    using GoldenRaspberryAwards.Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Producer> Producers => Set<Producer>();
        public DbSet<MovieProducer> MovieProducers => Set<MovieProducer>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Movie
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Year)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(m => m.Winner)
                .IsRequired();

            // Producer
            modelBuilder.Entity<Producer>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Producer>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(400);

            // MovieProducer
            modelBuilder.Entity<MovieProducer>()
                .HasKey(mp => new { mp.MovieId, mp.ProducerId });

            modelBuilder.Entity<MovieProducer>()
                .HasOne(mp => mp.Movie)
                .WithMany(m => m.MovieProducers)
                .HasForeignKey(mp => mp.MovieId);

            modelBuilder.Entity<MovieProducer>()
                .HasOne(mp => mp.Producer)
                .WithMany(p => p.MovieProducers)
                .HasForeignKey(mp => mp.ProducerId);
        }

    }

}
