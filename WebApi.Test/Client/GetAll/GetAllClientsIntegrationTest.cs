using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductClientHub.Communication.Responses;
using Shouldly;

namespace WebApi.Test.Client.GetAll;

public class GetAllClientsIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public GetAllClientsIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task GetAll_ShouldReturnSucess_WhenThereAreClientsInTheSystem()
    { 
        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Client 1",
                Email = "client1@email.com",
                Password = "password123"
            },
            new ClientEntity
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Client 2",
                Email = "client2@email.com",
                Password = "password456"
            }
        ];

        var response = await _httpClient.GetAsync("/api/clients"); 

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ResponseAllClientsJson>();

        body.ShouldNotBeNull();
        body!.Clients.Count.ShouldBe(2);
        body.Clients[0].Id.ShouldBe(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        body.Clients[0].Name.ShouldBe("Client 1");
        body.Clients[1].Id.ShouldBe(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        body.Clients[1].Name.ShouldBe("Client 2");
    }


    [Fact]
    public async Task GetAll_ShouldReturnNoContent_WhenThereAreNoClients()
    {
        _factory.ClientsToReturn = [];

        var response = await _httpClient.GetAsync("/api/clients");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
