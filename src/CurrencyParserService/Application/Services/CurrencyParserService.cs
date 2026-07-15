using Application.Interfaces;

namespace Application.Services;

public class CurrencyParserService(
    IDailyRateProvider dailyRateProvider,
    ICurrencyWriteRepository currencyWriteRepository)
    : ICurrencyParserService
{
    public async Task Parse(CancellationToken stoppingToken)
    {
        var dailyRates = await dailyRateProvider.GetDailyRates(stoppingToken);

        await currencyWriteRepository.SaveRates(dailyRates, stoppingToken);
    }
}
