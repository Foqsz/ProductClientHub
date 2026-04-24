using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Domain.Services.LoggedUser;
using ClientEntity = ProductClientHub.Domain.Entities.Client;

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
            services.RemoveAll<IClientWriteOnlyRepository>();
            services.RemoveAll<IAccessTokenValidator>();
            services.RemoveAll<IDeleteClientRepository>();
            services.RemoveAll<IUnitOfWork>();
            services.RemoveAll<ILoggedUser>();

            // Remove all hosted services to prevent background services from running during tests
            services.RemoveAll(typeof(IHostedService));

            services.AddScoped<IClientReadOnlyRepository, FakeClientReadOnlyRepository>();
            services.AddScoped<IClientWriteOnlyRepository, FakeClientWriteOnlyRepository>();
            services.AddScoped<IAccessTokenValidator, FakeAccessTokenValidator>();
            services.AddScoped<IDeleteClientRepository, FakeDeleteClientRepository>();
            services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
            services.AddScoped<ILoggedUser, FakeLoggedUser>();
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
        {
            var client = _clientStore.Clients.FirstOrDefault(c => c.Email == email);
            return Task.FromResult<ClientEntity?>(client);
        }

        public Task<IList<ClientEntity>> GetAll()
            => Task.FromResult<IList<ClientEntity>>(_clientStore.Clients);

        public Task<ClientEntity?> GetById(Guid clientId)
        {
            var client = _clientStore.Clients.FirstOrDefault(c => c.Id == clientId);
            return Task.FromResult<ClientEntity?>(client);
        }

        public Task<bool> ExistActiveClientWithIdentifier(Guid clientIdentifier) => Task.FromResult(true);
    }

    private sealed class FakeDeleteClientRepository : IDeleteClientRepository
    {
        private readonly TestClientStore _clientStore;

        public FakeDeleteClientRepository(TestClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public Task Delete(Guid clientId)
        {
            var client = _clientStore.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                _clientStore.Clients.Remove(client);
            }
            return Task.CompletedTask;
        }
    }

    private sealed class FakeClientWriteOnlyRepository : IClientWriteOnlyRepository
    {
        private readonly TestClientStore _clientStore;

        public FakeClientWriteOnlyRepository(TestClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public Task Add(ClientEntity client)
        {
            _clientStore.Clients.Add(client);
            return Task.CompletedTask;
        }

        public Task<ClientEntity?> Update(ClientEntity client)
        {
            var existingClient = _clientStore.Clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.Email = client.Email;
            }
            return Task.FromResult<ClientEntity?>(existingClient);
        }
    }

    private sealed class FakeLoggedUser : ILoggedUser
    {
        private readonly TestClientStore _clientStore;

        public FakeLoggedUser(TestClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public Task<ClientEntity> User()
        {
            var user = _clientStore.Clients.First();
            return Task.FromResult(user);
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task Commit() => Task.CompletedTask;
    }

    private sealed class TestClientStore
    {
        public IList<ClientEntity> Clients { get; set; } = [];
    }
}
