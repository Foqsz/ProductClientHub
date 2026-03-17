using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ProductClientHub.Domain.Extensions;

namespace ProductClientHub.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static void Migrate(IServiceProvider serviceProvider, string connectionString)
    {
        EnsureDatabaseCreated(connectionString);
        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated(string connectionString)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

        var dataBaseName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");

        using var dbConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", dataBaseName);

        var records = dbConnection.Query("SELECT 1 FROM pg_database WHERE datname = @name", parameters);

        if (records.Any().IsFalse())
            dbConnection.Execute($"CREATE DATABASE {dataBaseName}");

    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}
