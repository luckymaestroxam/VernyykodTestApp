using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Startup;

public static class StartupRepositories
{
    public static void AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserReadDbContext>(options =>
        {
            options.UseNpgsql(GetConnectionString(builder.Configuration));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        builder.Services.AddDbContext<UserWriteDbContext>(options =>
            options.UseNpgsql(GetConnectionString(builder.Configuration)));
        builder.Services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();
        builder.Services.AddScoped<IRevokedTokenWriteRepository, RevokedTokenWriteRepository>();
        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserWriteDbContext>());
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
