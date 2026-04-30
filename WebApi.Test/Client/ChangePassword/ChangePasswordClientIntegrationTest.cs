using Microsoft.AspNetCore.Mvc.Testing;
using CommonTestUtilities.Cryptografhy;
using ProductClientHub.Communication.Requests;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

namespace WebApi.Test.Client.ChangePassword;

public class ChangePasswordClientIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public ChangePasswordClientIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnNoContent_WhenRequestIsValid()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var passwordEncripter = PasswordEncripterBuilder.Build();

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "client1@email.com",
                Password = passwordEncripter.Encrypt("password123")
            }
        ];

        var request = new RequestChangePassword
        {
            CurrentPassword = "password123",
            NewPassword = "newPassword123"
        };

        var response = await _httpClient.PostAsync(
            $"/api/clients/changePassword/{clientId}",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnBadRequest_WhenCurrentPasswordIsInvalid()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var passwordEncripter = PasswordEncripterBuilder.Build();

        _factory.ClientsToReturn =
        [
            new ClientEntity
            {
                Id = clientId,
                Name = "Client 1",
                Email = "client1@email.com",
                Password = passwordEncripter.Encrypt("password123")
            }
        ];

        var request = new RequestChangePassword
        {
            CurrentPassword = "wrong-password",
            NewPassword = "newPassword123"
        };

        var response = await _httpClient.PostAsync(
            $"/api/clients/changePassword/{clientId}",
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.ShouldNotBeNullOrWhiteSpace();
    }
}
