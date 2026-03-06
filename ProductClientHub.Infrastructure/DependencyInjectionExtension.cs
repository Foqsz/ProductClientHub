using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        
    }

    private static void AddDbContext_SqlLite(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<ProductClientHubDbContext>(options =>
            options.UseSqlite(connectionString));
    }
}
