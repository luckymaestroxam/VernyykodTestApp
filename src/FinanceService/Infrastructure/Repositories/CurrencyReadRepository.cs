using Application.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CurrencyReadRepository(FinanceServiceReadDbContext dbContext) : ICurrencyReadRepository
{
    public Task<bool> Exists(CurrencyId currencyId, CancellationToken cancellationToken) =>
        dbContext.Currencies.AnyAsync(r => r.Id == currencyId.Value, cancellationToken);
}
