using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Product.Register;

public class RegisterProductIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public RegisterProductIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task RegisterProduct_ShouldReturnCreated_WhenRequestIsValid()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new RequestProductJson
        {
            Name = "Product 1",
            Brand = "Brand 1",
            Price = 10.5m
        };

        var response = await _httpClient.PostAsync(
            "/api/products",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var body = JsonConvert.DeserializeObject<ResponseShortProductJson>(await response.Content.ReadAsStringAsync());
        body.ShouldNotBeNull();
        body!.Name.ShouldBe(request.Name);
        body.Brand.ShouldBe(request.Brand);
        body.Price.ShouldBe(request.Price);
    }

    [Fact]
    public async Task RegisterProduct_ShouldReturnNotFound_WhenClientDoesNotExist()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var token = CommonTestUtilities.Token.JwtTokenGeneratorBuilder.Build().Generate(clientId);

        _factory.ClientsToReturn = [];

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new RequestProductJson
        {
            Name = "Product 1",
            Brand = "Brand 1",
            Price = 10.5m
        };

        var response = await _httpClient.PostAsync(
            "/api/products",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStringAsync();
        body.ShouldNotBeNullOrWhiteSpace();
    }
}
