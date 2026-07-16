using Application.Interfaces;

namespace Application.Services;

public class CurrencyParserService(
    IDailyRateProvider dailyRateProvider,
    ICurrencyRepository currencyRepository)
    : ICurrencyParserService
{
    public async Task Parse(CancellationToken stoppingToken)
    {
        var dailyRates = await dailyRateProvider.GetDailyRates(stoppingToken);

        await currencyRepository.SaveRates(dailyRates, stoppingToken);
    }
}
