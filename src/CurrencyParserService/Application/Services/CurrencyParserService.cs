using Application.Interfaces;

namespace Application.Services;

public class CurrencyParserService (IDailyRateParser dailyRateParser) : ICurrencyParserService
{
    public async Task Parse(CancellationToken stoppingToken)
    {
        var result = await dailyRateParser.GetDailyRates(stoppingToken);
    }
}
