using Application.Interfaces;
using Application.Models;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCurrencyReadRepository(FinanceServiceReadDbContext dbContext) : IUserCurrencyReadRepository
{
    public Task<bool> Exists(UserCurrency userCurrency, CancellationToken cancellationToken) =>
        dbContext.UserCurrencies
            .AnyAsync(u => u.UserId == userCurrency.UserId && u.CurrencyId == userCurrency.CurrencyId.Value,
                cancellationToken);

    public async Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken)
    {
        var userCurrenciesFromDb = await dbContext.UserCurrencies
            .Include(u => u.Currency)
            .Where(u => u.UserId == userId)
            .ToListAsync(cancellationToken);

        return userCurrenciesFromDb.Select(u => u.ToDto()).ToArray();
    }
}
