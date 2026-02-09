using AdvancedDevSample.Api.Tests.Integration;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using System.Net;
using System.Net.Http.Json;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class ProductAsyncControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryProductRepositoryAsync _repo;

        public ProductAsyncControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryProductRepositoryAsync)factory.Services.GetRequiredService<IProductRepositoryAsync>();
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NoContent_And_Save_Product()
        {
            // Arrange
            var product = new Product2(new Price(10));
            _repo.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 20 };

            // Act
            var response = await _client.PutAsJsonAsync(
                $"/api/productasync/{product.Id}/price",
                request
            );

            // Assert - HTTP
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert - Péristence réelle
            var updated = await _repo.GetByIdAsync(product.Id);
            Assert.Equal(20, updated.Price.Value);

        }

    }
}