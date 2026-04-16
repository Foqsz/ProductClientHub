using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net.Http.Headers;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Client.GetById;

public class GetByIdClientIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public GetByIdClientIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task GetClientById_ShouldReturnSucess()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "client@gmail.com",
                Password = "teste123",
            }
        ];

        var response = await _httpClient.GetAsync($"/api/clients/{clientId}");

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetClientById_ShouldReturnNotFound()
    {
        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                Name = "Client 1",
                Email = "clientt@gmail.com",
                Password = "teste123",
            }
        ];

        var response = await _httpClient.GetAsync($"/api/clients/{Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")}");

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }
}
