using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Users;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Products;
using ProductClientHub.Infrastructure.DataAcess.UnitOfWork;
using ProductClientHub.Infrastructure.Database;
using ProductClientHub.Infrastructure.Extensions;
using ProductClientHub.Infrastructure.Security.Cryptography;
using System.Reflection;

namespace ProductClientHub.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext_PostgreSql(services, configuration);
        AddFluentMigrator_PostgreSql(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //clients
        services.AddScoped<IClientWriteOnlyRepository, ClientWriteOnlyRepository>();
        services.AddScoped<IClientReadOnlyRepository, ClientReadOnlyRepository>();
        services.AddScoped<IDeleteClientRepository, ClientWriteOnlyRepository>();
        services.AddScoped<IPasswordEncripter, BCryptNet>();

        //products
        services.AddScoped<IProductsWriteOnlyRepository, ProductsWriteOnlyRepository>();
        services.AddScoped<IProductsReadOnlyRepository, ProductsReadOnlyRepository>();
        services.AddScoped<IDeleteProductOnlyRepository, ProductsWriteOnlyRepository>();
    }

    private static void AddDbContext_PostgreSql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<ProductClientHubDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddFluentMigrator_PostgreSql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore()
            .ConfigureRunner(options => options
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("ProductClientHub.Infrastructure")).For.All()
            );
    }
}
