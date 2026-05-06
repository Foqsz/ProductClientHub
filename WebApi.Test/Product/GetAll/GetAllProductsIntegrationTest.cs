using Microsoft.AspNetCore.Mvc.Testing;
using ProductClientHub.Communication.Responses;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductEntity = ProductClientHub.Domain.Entities.Product;

namespace WebApi.Test.Product.GetAll;

public class GetAllProductsIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public GetAllProductsIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task GetAll_ShouldReturnSuccess_WhenThereAreProductsInTheSystem()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

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
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Product 1",
                Brand = "Brand 1",
                Price = 10.5m,
                ClientId = clientId,
                Client = _factory.ClientsToReturn[0]
            },
            new ProductEntity
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Name = "Product 2",
                Brand = "Brand 2",
                Price = 25m,
                ClientId = clientId,
                Client = _factory.ClientsToReturn[0]
            }
        ];

        var response = await _httpClient.GetAsync("/api/products");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ResponseProductsJson>();

        body.ShouldNotBeNull();
        body!.Products.Count.ShouldBe(2);
        body.Products[0].Id.ShouldBe(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        body.Products[0].Name.ShouldBe("Product 1");
        body.Products[0].Client.Id.ShouldBe(clientId);
        body.Products[1].Id.ShouldBe(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        body.Products[1].Name.ShouldBe("Product 2");
        body.Products[1].Client.Id.ShouldBe(clientId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnNotFound_WhenThereAreNoProducts()
    {
        _factory.ProductsToReturn = [];

        var response = await _httpClient.GetAsync("/api/products");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
