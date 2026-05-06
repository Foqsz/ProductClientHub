using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductEntity = ProductClientHub.Domain.Entities.Product;

namespace WebApi.Test.Product.Delete;

public class DeleteProductIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public DeleteProductIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenProductExists()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var productId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var token = CommonTestUtilities.Token.JwtTokenGeneratorBuilder.Build().Generate(clientId);

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "client1@email.com",
                Password = "password123",
                Products = []
            }
        ];

        _factory.ProductsToReturn =
        [
            new ProductEntity
            {
                Id = productId,
                Name = "Product 1",
                Brand = "Brand 1",
                Price = 10.5m,
                ClientId = clientId,
                Client = _factory.ClientsToReturn[0]
            }
        ];

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"/api/products/{productId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenProductDoesNotExist()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var productId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var token = CommonTestUtilities.Token.JwtTokenGeneratorBuilder.Build().Generate(clientId);

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "client1@email.com",
                Password = "password123",
                Products = []
            }
        ];

        _factory.ProductsToReturn = [];

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"/api/products/{productId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
