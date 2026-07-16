using Domain.Aggregates;

namespace Application.Interfaces;

public interface ICurrencyRepository
{
    Task SaveRates(Currency[] currencies, CancellationToken stoppingToken);
}
