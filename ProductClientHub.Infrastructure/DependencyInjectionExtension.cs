using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NpgsqlTypes;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Products;
using ProductClientHub.Infrastructure.DataAcess.Repositories.Users;
using ProductClientHub.Infrastructure.DataAcess.UnitOfWork;
using ProductClientHub.Infrastructure.Database;
using ProductClientHub.Infrastructure.Extensions;
using ProductClientHub.Infrastructure.Security.Cryptography;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System.Reflection;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Infrastructure.Security.Tokens.Acess.Generator;
using ProductClientHub.Infrastructure.Security.Tokens.Acess.Validator;
using ProductClientHub.Domain.Services.LoggedUser;
using ProductClientHub.Domain.Services.Messaging;
using ProductClientHub.Infrastructure.Services;
using ProductClientHub.Infrastructure.Messaging.RabbitMq;

namespace ProductClientHub.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext_PostgreSql(services, configuration);
        AddFluentMigrator_PostgreSql(services, configuration);
        AddTokens(services, configuration);
        AddPasswordEncrpter(services);
        AddLoggedUser(services);
        AddMessaging(services, configuration);
    }

    public static void AddLogger(this IHostBuilder builder, IConfiguration configuration)
    {
        AddSerilog(builder, configuration);
    }

    private static void AddPasswordEncrpter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //clients
        services.AddScoped<IClientWriteOnlyRepository, ClientWriteOnlyRepository>();
        services.AddScoped<IClientReadOnlyRepository, ClientReadOnlyRepository>();
        services.AddScoped<IDeleteClientRepository, ClientWriteOnlyRepository>();

        //products
        services.AddScoped<IProductsWriteOnlyRepository, ProductsWriteOnlyRepository>();
        services.AddScoped<IProductsReadOnlyRepository, ProductsReadOnlyRepository>();
        services.AddScoped<IDeleteProductOnlyRepository, ProductsWriteOnlyRepository>();
        services.AddScoped<IUploadProductOnlyRepository, ProductsWriteOnlyRepository>();
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        //services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();
        if (rabbitMqOptions?.EnableConsumer is not false)
        {
            services.AddHostedService<ClientCreatedConsumer>();
        }
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

    private static void AddSerilog(IHostBuilder builder, IConfiguration configuration)
    {
        var outputTemplate = "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";
        var connectionString = configuration.ConnectionString();
        var tableName = "logs";
        var fileName = "logs/log-.txt";

        IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
        };

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File(fileName, rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate, restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.PostgreSQL(connectionString, tableName, columnWriters, restrictedToMinimumLevel: LogEventLevel.Error , needAutoCreateTable: true)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.UseSerilog();
    }
}
