using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Persistence;
using EFCore.BulkExtensions;
using Infrastructure.Dtos;

namespace Infrastructure.Repositories;

public class CurrencyWriteRepository(CurrencyDbContext currencyDbContext) : ICurrencyWriteRepository
{
    public Task SaveRates(Currency[] currencies, CancellationToken stoppingToken)
    {
        return currencyDbContext.BulkInsertOrUpdateAsync(currencies.Select(c => c.ToDto()),
            cancellationToken: stoppingToken);
    }
}
