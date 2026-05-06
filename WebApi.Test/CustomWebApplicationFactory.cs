using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;
using ClientEntity = ProductClientHub.Domain.Entities.Client;
using ProductEntity = ProductClientHub.Domain.Entities.Product;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly TestClientStore _clientStore = new();
    private readonly TestProductStore _productStore = new();

    public IList<ClientEntity> ClientsToReturn
    {
        get => _clientStore.Clients;
        set => _clientStore.Clients = value;
    }

    public IList<ProductEntity> ProductsToReturn
    {
        get => _productStore.Products;
        set => _productStore.Products = value;
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
            services.AddSingleton(_productStore);

            services.RemoveAll<IClientReadOnlyRepository>();
            services.RemoveAll<IClientWriteOnlyRepository>();
            services.RemoveAll<IAccessTokenValidator>();
            services.RemoveAll<IDeleteClientRepository>();
            services.RemoveAll<IUnitOfWork>();
            services.RemoveAll<ILoggedClient>();
            services.RemoveAll<IProductsReadOnlyRepository>();
            services.RemoveAll<IProductsWriteOnlyRepository>();
            services.RemoveAll<IDeleteProductWriteOnlyRepository>();
            services.RemoveAll<IUpdateProductOnlyRepository>();

            // Remove all hosted services to prevent background services from running during tests
            services.RemoveAll(typeof(IHostedService));

            services.AddScoped<IClientReadOnlyRepository, FakeClientReadOnlyRepository>();
            services.AddScoped<IClientWriteOnlyRepository, FakeClientWriteOnlyRepository>();
            services.AddScoped<IAccessTokenValidator, FakeAccessTokenValidator>();
            services.AddScoped<IDeleteClientRepository, FakeDeleteClientRepository>();
            services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
            services.AddScoped<ILoggedClient, FakeloggedClient>();
            services.AddScoped<IProductsReadOnlyRepository, FakeProductsReadOnlyRepository>();
            services.AddScoped<IProductsWriteOnlyRepository, FakeProductsWriteOnlyRepository>();
            services.AddScoped<IDeleteProductWriteOnlyRepository, FakeProductsWriteOnlyRepository>();
            services.AddScoped<IUpdateProductOnlyRepository, FakeProductsWriteOnlyRepository>();
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

    private sealed class FakeProductsReadOnlyRepository : IProductsReadOnlyRepository
    {
        private readonly TestProductStore _productStore;

        public FakeProductsReadOnlyRepository(TestProductStore productStore)
        {
            _productStore = productStore;
        }

        public Task<IList<ProductEntity>> GetAll()
            => Task.FromResult<IList<ProductEntity>>(_productStore.Products);

        public Task<ProductEntity?> GetById(Guid productId)
        {
            var product = _productStore.Products.FirstOrDefault(p => p.Id == productId);
            return Task.FromResult<ProductEntity?>(product);
        }
    }

    private sealed class FakeProductsWriteOnlyRepository : IProductsWriteOnlyRepository, IDeleteProductWriteOnlyRepository, IUpdateProductOnlyRepository
    {
        private readonly TestProductStore _productStore;
        private readonly TestClientStore _clientStore;

        public FakeProductsWriteOnlyRepository(TestProductStore productStore, TestClientStore clientStore)
        {
            _productStore = productStore;
            _clientStore = clientStore;
        }

        public Task Add(ProductEntity product)
        {
            if (product.Client is null)
            {
                var client = _clientStore.Clients.FirstOrDefault(c => c.Id == product.ClientId);
                if (client is not null)
                {
                    product.Client = client;
                    client.Products.Add(product);
                }
            }

            _productStore.Products.Add(product);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(Guid clientId, Guid productId)
        {
            var product = _productStore.Products.FirstOrDefault(p => p.Id == productId && p.ClientId == clientId);
            if (product is null)
            {
                return Task.FromResult(false);
            }

            _productStore.Products.Remove(product);
            var client = _clientStore.Clients.FirstOrDefault(c => c.Id == clientId);
            client?.Products.Remove(product);
            return Task.FromResult(true);
        }

        public Task Update(Guid clientId, Guid productId, ProductEntity product)
        {
            var existingProduct = _productStore.Products.FirstOrDefault(p => p.Id == productId && p.ClientId == clientId);
            if (existingProduct is null)
            {
                return Task.CompletedTask;
            }

            existingProduct.Name = product.Name;
            existingProduct.Brand = product.Brand;
            existingProduct.Price = product.Price;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeloggedClient : ILoggedClient
    {
        private readonly TestClientStore _clientStore;

        public FakeloggedClient(TestClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public Task<ClientEntity> User()
        {
            var user = _clientStore.Clients.FirstOrDefault();

            if (user is null)
            {
                throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);
            }

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

    private sealed class TestProductStore
    {
        public IList<ProductEntity> Products { get; set; } = [];
    }
}
