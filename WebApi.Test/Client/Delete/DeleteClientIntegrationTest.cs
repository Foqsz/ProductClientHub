using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net.Http.Headers;

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
    public async Task Delete_ShouldReturnNoContent_WhenClientIsDeleted()
    {
        var clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        _factory.ClientsToReturn =
        [
            new ProductClientHub.Domain.Entities.Client
            {
                Id = clientId,
                Name = "Client 1",
                Email = "oioi@gmail.com",
                Password = "password123",
                Products = new List<ProductClientHub.Domain.Entities.Product>()
            }
        ];

        var response = await _httpClient.DeleteAsync($"/api/clients/{clientId}");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }
}