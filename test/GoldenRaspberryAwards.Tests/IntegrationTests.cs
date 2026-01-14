using FluentAssertions;
using GoldenRaspberryAwards.Api.Application.Commands;
using System.Net;
using System.Text.Json;

namespace GoldenRaspberryAwards.Tests.Integration
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return 200 OK")]
        public async Task GetIntervals_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/producers/intervals");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return valid API contract")]
        public async Task GetIntervals_ShouldReturnValidContract()
        {
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            result.Should().NotBeNull();

            result!.Min.Should().NotBeNull().And.NotBeEmpty();
            result.Max.Should().NotBeNull().And.NotBeEmpty();

            result.Min.Concat(result.Max).Should().OnlyContain(item =>
                item.Interval >= 0 &&
                item.FollowingWin >= item.PreviousWin &&
                item.Producer != null
            );
        }

        [Fact(DisplayName = "Min interval should represent the smallest interval between consecutive awards")]
        public async Task MinInterval_ShouldBeSmallestInterval()
        {
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            var smallestInterval = result.Min.Min(p => p.Interval);

            result.Min.Should().OnlyContain(p => p.Interval == smallestInterval);
        }

        [Fact(DisplayName = "Max interval should represent the largest interval between consecutive awards")]
        public async Task MaxInterval_ShouldBeLargestInterval()
        {
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            var largestInterval = result.Max.Max(p => p.Interval);

            result.Max.Should().OnlyContain(p => p.Interval == largestInterval);
        }

        [Fact(DisplayName = "Interval should match the difference between followingWin and previousWin")]
        public async Task Interval_ShouldMatchDifferenceBetweenYears()
        {
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            foreach (var item in result.Min.Concat(result.Max))
            {
                item.Interval.Should().Be(item.FollowingWin - item.PreviousWin);
            }
        }
    }
}
