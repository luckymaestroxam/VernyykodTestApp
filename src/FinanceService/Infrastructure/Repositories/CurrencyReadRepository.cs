using FinanceService.Application.Interfaces;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.ValueObjects;

namespace FinanceService.Infrastructure.Repositories;

public class CurrencyReadRepository(FinanceServiceReadDbContext dbContext) : ICurrencyReadRepository
{
    public Task<bool> Exists(CurrencyId currencyId, CancellationToken cancellationToken) =>
        dbContext.Currencies.AnyAsync(r => r.Id == currencyId.Value, cancellationToken);
}
