using Shared.Domain.Aggregates;

namespace CurrencyParserService.Application.Interfaces;

public interface IDailyRateProvider
{
    Task<Currency[]> GetDailyRates(CancellationToken stoppingToken);
}
