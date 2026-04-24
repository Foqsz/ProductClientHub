using CommonTestUtilities.Token;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Client.Update;

public class UpdateClientIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public UpdateClientIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task UpdateClientTest_Sucess()
    {
        var client = _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Name = "Update Client",
                Email = "updateclientTEST@gmail.com"
            }, 
        ];

        client[0].Id = Guid.NewGuid();

        var token = JwtTokenGeneratorBuilder.Build().Generate(client[0].Id);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsync($"/api/clients", new StringContent(JsonConvert.SerializeObject(client[0]), System.Text.Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateClientTest_Error_EmailExist()
    {
        var client = _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Name = "User Logado",
                Email = "user1@gmail.com"
            },

            new ClientEntity
            {
                Name = "Outro User",
                Email = "user2@gmail.com"
            }
        ];

        client[0].Id = Guid.NewGuid();
        client[1].Id = Guid.NewGuid();

        var token = JwtTokenGeneratorBuilder.Build().Generate(client[0].Id);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var updateRequest = new ClientEntity
        {
            Id = client[0].Id,
            Name = "Update",
            Email = client[1].Email  
        };

        var response = await _httpClient.PutAsync(
            "/api/clients",
            new StringContent(JsonConvert.SerializeObject(updateRequest),
            Encoding.UTF8,
            "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
