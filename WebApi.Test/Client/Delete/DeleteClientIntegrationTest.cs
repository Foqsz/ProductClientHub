using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Client.Delete;

public class DeleteClientIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public DeleteClientIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }


    [Fact]
    public async Task DeleteClient_ShouldReturnNoContent_WhenClientExists()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "oioi@gmail.com",
                Password = "password123",
                Products = []
            }
        ];

        var response = await _httpClient.DeleteAsync($"/api/clients/{clientId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteClient_ShouldReturnNotFound_WhenClientDoesNotExist()
    {
        var nonExistentClientId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        _factory.ClientsToReturn = [];

        var response = await _httpClient.DeleteAsync($"/api/clients/{nonExistentClientId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStringAsync();
        body.ShouldNotBeNullOrWhiteSpace();
    }
}