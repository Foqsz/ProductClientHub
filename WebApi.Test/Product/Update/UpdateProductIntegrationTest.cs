using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductClientHub.Communication.Requests;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductEntity = ProductClientHub.Domain.Entities.Product;

namespace WebApi.Test.Product.Update;

public class UpdateProductIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public UpdateProductIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnOk_WhenRequestIsValid()
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

        var request = new RequestProductJson
        {
            Name = "Product Updated",
            Brand = "Brand Updated",
            Price = 20m
        };

        var response = await _httpClient.PutAsync(
            $"/api/products/{productId}",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
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

        var request = new RequestProductJson
        {
            Name = "Product Updated",
            Brand = "Brand Updated",
            Price = 20m
        };

        var response = await _httpClient.PutAsync(
            $"/api/products/{productId}",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
