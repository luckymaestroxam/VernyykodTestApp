using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Startup;

public static class StartupRepositories
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CurrencyDbContext>(options => options.UseNpgsql(GetConnectionString(configuration)));
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("connections:postgres:host");
        var port = configuration.GetValue<string>("connections:postgres:port");
        var username = configuration.GetValue<string>("connections:postgres:username");
        var password = configuration.GetValue<string>("connections:postgres:password");
        var database = configuration.GetValue<string>("connections:postgres:database");

        return $"host={host};port={port};username={username};password={password};database={database}";
    }
}
