using Shared.Domain.Aggregates;

namespace CurrencyParserService.Application.Interfaces;

public interface ICurrencyRepository
{
    Task SaveRates(Currency[] currencies, CancellationToken stoppingToken);
}
