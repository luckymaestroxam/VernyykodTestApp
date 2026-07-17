using CurrencyParserService.ConsoleApp.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyParserService.ConsoleApp.Startup;

public static class StartupCurrencyParserBackgroundService
{
    public static IServiceCollection AddCurrencyParserBackgroundService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var delayText = configuration["currencyParserBackgroundService:delayInSeconds"];
        if (!int.TryParse(delayText, out var delayInSeconds) || delayInSeconds <= 0)
        {
            throw new InvalidOperationException(
                "Не задан корректный currencyParserBackgroundService:delayInSeconds.");
        }

        services.AddHostedService(sp => new CurrencyParserBackgroundService(
            sp.GetRequiredService<IServiceScopeFactory>(),
            delayInSeconds));

        return services;
    }
}
