using Microsoft.AspNetCore.Mvc.Testing;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Newtonsoft.Json;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Client.Register;

public class RegisterClientIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public RegisterClientIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task RegisterClient_ShouldReturnCreated_WhenRequestIsValid()
    {
        _factory.ClientsToReturn = [];

        var request = new RequestClientJson
        {
            Name = "Client Register",
            Email = "client.register@email.com",
            Password = "password123"
        };

        var response = await _httpClient.PostAsync(
            "/api/register",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var body = JsonConvert.DeserializeObject<ResponseShortClientJson>(await response.Content.ReadAsStringAsync());
        body.ShouldNotBeNull();
        body!.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task RegisterClient_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Existing Client",
                Email = "existing@email.com",
                Password = "password123"
            }
        ];

        var request = new RequestClientJson
        {
            Name = "New Client",
            Email = "existing@email.com",
            Password = "password123"
        };

        var response = await _httpClient.PostAsync(
            "/api/register",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.ShouldNotBeNullOrWhiteSpace();
    }
}
