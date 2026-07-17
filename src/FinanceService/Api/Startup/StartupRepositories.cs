using FinanceService.Application.Interfaces;
using FinanceService.Infrastructure.Persistence;
using FinanceService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;

namespace FinanceService.Api.Startup;

public static class StartupRepositories
{
    public static void AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FinanceServiceReadDbContext>(options =>
        {
            options.UseNpgsql(GetConnectionString(builder.Configuration));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        builder.Services.AddDbContext<FinanceServiceWriteDbContext>(options =>
        {
            options.UseNpgsql(GetConnectionString(builder.Configuration));
        });
        builder.Services.AddScoped<IRevokedTokenReadRepository, RevokedTokenReadRepository>();
        builder.Services.AddScoped<IUserCurrencyReadRepository, UserCurrencyReadRepository>();
        builder.Services.AddScoped<IUserCurrencyWriteRepository, UserCurrencyWriteRepository>();
        builder.Services.AddScoped<ICurrencyReadRepository, CurrencyReadRepository>();
        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<FinanceServiceWriteDbContext>());
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
