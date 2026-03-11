using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Clients;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Products;
using ProductClientHub.Infrastructure.DataAcess.UnitOfWork;
using ProductClientHub.Infrastructure.Database;
using ProductClientHub.Infrastructure.Extensions;

namespace ProductClientHub.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext_SqlLite(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //clients
        services.AddScoped<IClientWriteOnlyRepository, ClientWriteOnlyRepository>();
        services.AddScoped<IClientReadOnlyRepository, ClientReadOnlyRepository>();

        //products
        services.AddScoped<IProductsWriteOnlyRepository, ProductsWriteOnlyRepository>();
    }

    private static void AddDbContext_SqlLite(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<ProductClientHubDbContext>(options =>
            options.UseSqlite(connectionString));
    }
}
