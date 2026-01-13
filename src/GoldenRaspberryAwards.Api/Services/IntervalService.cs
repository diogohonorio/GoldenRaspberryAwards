using GoldenRaspberryAwards.Api.Application.Commands;
using GoldenRaspberryAwards.Api.Data;
using GoldenRaspberryAwards.Api.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Api.Services
{
    public class IntervalService
    {
        private readonly AppDbContext _db;

        public IntervalService(AppDbContext db)
        {
            _db = db;
        }

        public ProducersIntervalResponse Calculate()
        {
            var winners = _db.Movies
                .Where(m => m.Winner)
                .Include(m => m.MovieProducers)
                    .ThenInclude(mp => mp.Producer)
                .AsEnumerable()
                .SelectMany(m => m.MovieProducers.Select(mp => new
                {
                    Producer = mp.Producer.Name.Trim(),
                    m.Year
                }))
                .GroupBy(x => x.Producer)
                .Where(g => g.Count() > 1)
                .ToList();

            var intervals = new List<AwardIntervalDto>();

            foreach (var group in winners)
            {
                var years = group
                    .Select(x => x.Year)
                    .OrderBy(y => y)
                    .ToList();

                for (int i = 1; i < years.Count; i++)
                {
                    intervals.Add(new AwardIntervalDto
                    {
                        Producer = group.Key,
                        PreviousWin = years[i - 1],
                        FollowingWin = years[i],
                        Interval = years[i] - years[i - 1]
                    });
                }
            }

            if (!intervals.Any())
                return new ProducersIntervalResponse();

            var min = intervals.Min(x => x.Interval);
            var max = intervals.Max(x => x.Interval);

            return new ProducersIntervalResponse
            {
                Min = intervals.Where(x => x.Interval == min).ToList(),
                Max = intervals.Where(x => x.Interval == max).ToList()
            };
        }
    }

}
