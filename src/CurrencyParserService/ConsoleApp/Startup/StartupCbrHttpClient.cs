using CurrencyParserService.Application.Interfaces;
using CurrencyParserService.Infrastructure.HttpClients;
using CurrencyParserService.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyParserService.ConsoleApp.Startup;

public static class StartupCbrHttpClient
{
    public static IServiceCollection AddCbrHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["cbr:http:baseUrl"];
        var xmlDailyUrl = configuration["cbr:http:xmldailyurl"];

        var options = new CbrOptions(new Uri(new Uri(baseUrl!), xmlDailyUrl).ToString());
        services.AddSingleton(options);
        services.AddHttpClient<IDailyRateProvider, CbrHttpClient>()
            .AddStandardResilienceHandler(resilience =>
            {
                resilience.Retry.MaxRetryAttempts = 3;
            });

        return services;
    }
}
