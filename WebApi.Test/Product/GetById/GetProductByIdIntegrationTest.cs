using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductEntity = ProductClientHub.Domain.Entities.Product;

namespace WebApi.Test.Product.GetById;

public class GetProductByIdIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public GetProductByIdIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task GetById_ShouldReturnSuccess_WhenProductExists()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var productId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

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

        var response = await _httpClient.GetAsync($"/api/products/{productId}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var productId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        _factory.ProductsToReturn = [];

        var response = await _httpClient.GetAsync($"/api/products/{productId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
