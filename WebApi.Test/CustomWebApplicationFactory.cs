using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Security.Tokens;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly TestClientStore _clientStore = new();

    public IList<ClientEntity> ClientsToReturn
    {
        get => _clientStore.Clients;
        set => _clientStore.Clients = value;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Connection"] = "Host=localhost;Port=5432;Database=ignored;Username=ignored;Password=ignored",
                ["Jwt:SigningKey"] = "integration_tests_signing_key_12345678901234567890",
                ["Jwt:ExpirationTimeMinutes"] = "60",
                ["RabbitMQ:EnableConsumer"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(_clientStore);

            services.RemoveAll<IClientReadOnlyRepository>();
            services.RemoveAll<IAccessTokenValidator>();

            services.AddScoped<IClientReadOnlyRepository, FakeClientReadOnlyRepository>();
            services.AddScoped<IAccessTokenValidator, FakeAccessTokenValidator>();
        });
    }

    private sealed class FakeAccessTokenValidator : IAccessTokenValidator
    {
        public Guid ValidateAnGetUserIdentifier(string token)
            => Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    private sealed class FakeClientReadOnlyRepository : IClientReadOnlyRepository
    {
        private readonly TestClientStore _clientStore;

        public FakeClientReadOnlyRepository(TestClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public Task<ClientEntity?> EmailAlreadyExists(string email)
            => Task.FromResult<ClientEntity?>(null);

        public Task<IList<ClientEntity>> GetAll()
            => Task.FromResult<IList<ClientEntity>>(_clientStore.Clients);

        public Task<ClientEntity?> GetById(Guid clientId)
        {
            var client = _clientStore.Clients.FirstOrDefault(c => c.Id == clientId);
            return Task.FromResult<ClientEntity?>(client);
        }

        public Task<bool> ExistActiveClientWithIdentifier(Guid clientIdentifier) => Task.FromResult(true);
    }

    private sealed class TestClientStore
    {
        public IList<ClientEntity> Clients { get; set; } = [];
    }
}
