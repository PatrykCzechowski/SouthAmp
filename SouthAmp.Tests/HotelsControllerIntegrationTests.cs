using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SouthAmp.Application.DTOs;
using Xunit;

namespace SouthAmp.Tests
{
    public class HotelsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public HotelsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllHotels_ShouldReturnOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/hotels");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddHotel_Unauthorized_ShouldReturn401()
        {
            var client = _factory.CreateClient();
            var hotel = new HotelDto { Name = "Test", City = "TestCity", Country = "TestCountry" };
            var response = await client.PostAsJsonAsync("/api/hotels", hotel);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}