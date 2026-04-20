using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Net.Http.Headers;
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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task UpdateClientTest_Sucess()
    {
        var client = _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Name = "Update Client",
                Email = "updateclient@gmail.com"
            }
        ];

        client[0].Id = Guid.NewGuid();

        var response = await _httpClient.PutAsync($"/api/clients/{client[0].Id}", new StringContent(JsonConvert.SerializeObject(client[0]), System.Text.Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateClientTest_Error_EmailExist()
    {
        var client = _factory.ClientsToReturn =
        [
            new ClientEntity
                {
                    Name = "Update Client",
                    Email = "updateclient@gmail.com"
                },

            new ClientEntity
                {
                    Name = "Update Client 2",
                    Email = "updateclient@gmail.com"
                }
        ];

        client[0].Id = Guid.NewGuid();
        client[1].Id = Guid.NewGuid();

        var response = await _httpClient.PutAsync($"/api/clients/{client[1].Id}", new StringContent(JsonConvert.SerializeObject(client[1]), System.Text.Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
    }
}
