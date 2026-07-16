using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Startup;

public static class StartupServices
{
    public static void AddServices(this IServiceCollection services) =>
        services.AddScoped<ICurrencyParserService, CurrencyParserService>();
}
