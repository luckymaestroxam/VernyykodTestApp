using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ConsoleApp.Startup;

public static class StartupMigrations
{
    public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = BuildConnectionStringForMigration(configuration);

        return services.AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(StartupMigrations).Assembly).For.Migrations())
            .AddLogging(c => c.AddFluentMigratorConsole());
    }

    public static async Task MigrateDatabase(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        await EnsureDatabase(configuration);

        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    private static async Task EnsureDatabase(IConfiguration configuration)
    {
        var database = configuration["connections:postgres:database"];
        var connectionString = BuildConnectionStringForCreateDatabase(configuration);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var existsCommand = new NpgsqlCommand(
            "select exists(select 1 from pg_database where datname = @database)", connection);
        existsCommand.Parameters.AddWithValue("database", database!);
        if (await existsCommand.ExecuteScalarAsync() is true)
        {
            return;
        }

        await using var createCommand = new NpgsqlCommand($"create database {database}", connection);
        await createCommand.ExecuteNonQueryAsync();
    }

    private static string BuildConnectionStringForCreateDatabase(IConfiguration configuration)
    {
        var host = configuration["connections:postgres:host"];
        var port = configuration["connections:postgres:port"];
        var username = configuration["connections:postgres:username"];
        var password = configuration["connections:postgres:password"];

        return $"Host={host};Port={port};Username={username};Password={password};Database=postgres;";
    }

    private static string BuildConnectionStringForMigration(IConfiguration configuration)
    {
        var host = configuration["connections:postgres:host"];
        var port = configuration["connections:postgres:port"];
        var username = configuration["connections:postgres:username"];
        var password = configuration["connections:postgres:password"];
        var database = configuration["connections:postgres:database"];

        return $"Host={host};Port={port};Username={username};Password={password};Database={database};";
    }
}
