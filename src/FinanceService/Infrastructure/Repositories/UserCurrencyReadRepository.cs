using Application.Interfaces;
using Application.Models;
using Infrastructure.Mappers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCurrencyReadRepository(FinanceServiceReadDbContext userReadDbContext) : IUserCurrencyReadRepository
{
    public async Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken)
    {
        var userCurrenciesFromDb = await userReadDbContext.UserCurrencies
            .Include(u => u.Currency)
            .Where(u => u.UserId == userId)
            .ToListAsync(cancellationToken);

        return userCurrenciesFromDb.Select(u => u.ToDto()).ToArray();
    }
}
