using CurrencyParserService.Application.Interfaces;
using CurrencyParserService.Infrastructure.Mappers;
using CurrencyParserService.Infrastructure.Persistence;
using EFCore.BulkExtensions;
using Shared.Domain.Aggregates;

namespace CurrencyParserService.Infrastructure.Repositories;

public class CurrencyRepository(CurrencyDbContext currencyDbContext) : ICurrencyRepository
{
    public Task SaveRates(Currency[] currencies, CancellationToken stoppingToken) =>
        currencyDbContext.BulkInsertOrUpdateAsync(currencies.Select(c => c.ToEntity()),
            cancellationToken: stoppingToken);
}
