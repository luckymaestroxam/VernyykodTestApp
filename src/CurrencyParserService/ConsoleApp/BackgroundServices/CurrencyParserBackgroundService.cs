using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp.BackgroundServices;

public class CurrencyParserBackgroundService(
    IServiceScopeFactory scopeFactory,
    int delayInSeconds) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ParseSafe(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(delayInSeconds));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ParseSafe(stoppingToken);
        }
    }

    private async Task ParseSafe(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var currencyParserService = scope.ServiceProvider.GetRequiredService<ICurrencyParserService>();
            await currencyParserService.Parse(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при парсинге курсов валют. {ex.Message}");
        }
    }
}
