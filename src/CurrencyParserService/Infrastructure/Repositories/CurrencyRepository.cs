using Application.Interfaces;
using Domain.Aggregates;
using EFCore.BulkExtensions;
using Infrastructure.Mappers;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class CurrencyRepository(CurrencyDbContext currencyDbContext) : ICurrencyRepository
{
    public Task SaveRates(Currency[] currencies, CancellationToken stoppingToken) =>
        currencyDbContext.BulkInsertOrUpdateAsync(currencies.Select(c => c.ToEntity()),
            cancellationToken: stoppingToken);
}
