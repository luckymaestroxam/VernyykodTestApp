using Domain.Aggregates;

namespace Application.Interfaces;

public interface ICurrencyWriteRepository
{
    Task SaveRates(Currency[] currencies, CancellationToken stoppingToken);
}
