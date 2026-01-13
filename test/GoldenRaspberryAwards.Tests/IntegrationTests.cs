using FluentAssertions;
using GoldenRaspberryAwards.Api.Application.Commands;
using System.Net;
using System.Text.Json;

namespace GoldenRaspberryAwards.Tests.Integration
{
    public partial class IntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return 200 OK")]
        public async Task GetIntervals_ShouldReturn200()
        {
            // Act
            var response = await _client.GetAsync("/api/producers/intervals");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("min").And.Contain("max");
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return Min and Max")]
        public async Task GetIntervals_ShouldReturnMinAndMax()
        {
            // Act
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.Should().NotBeNull();
            result!.Min.Should().NotBeNull();
            result.Max.Should().NotBeNull();
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return at least one producer in Min and Max")]
        public async Task GetIntervals_ShouldReturnAtLeastOneProducerInMinAndMax()
        {
            // Act
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.Should().NotBeNull();
            result!.Min.Should().NotBeEmpty();
            result.Max.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return valid intervals")]
        public async Task GetIntervals_ShouldReturnValidIntervals()
        {
            // Act
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.Should().NotBeNull();
            result!.Min.Should().NotBeNull();
            result.Max.Should().NotBeNull();

            foreach (var item in result.Min!)
            {
                item.Interval.Should().BeGreaterThanOrEqualTo(0);

                if (!string.IsNullOrWhiteSpace(item.Producer))
                {
                    item.Producer.Should().NotBeNullOrWhiteSpace();
                }
            }

            foreach (var item in result.Max!)
            {
                item.Interval.Should().BeGreaterThanOrEqualTo(0);

                if (!string.IsNullOrWhiteSpace(item.Producer))
                {
                    item.Producer.Should().NotBeNullOrWhiteSpace();
                }
            }
        }

        [Fact(DisplayName = "GET /api/producers/intervals should return non-empty min and max lists")]
        public async Task GetIntervals_ShouldReturnNonEmptyMinAndMaxLists()
        {
            // Act
            var response = await _client.GetAsync("/api/producers/intervals");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProducersIntervalResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.Should().NotBeNull();

            result!.Min.Should().NotBeNull().And.NotBeEmpty();
            result.Max.Should().NotBeNull().And.NotBeEmpty();

            foreach (var item in result.Min)
                item.Interval.Should().BeGreaterThanOrEqualTo(0);

            foreach (var item in result.Max)
                item.Interval.Should().BeGreaterThanOrEqualTo(0);
        }

    }
}