using Application.Interfaces;
using Application.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCurrencyReadRepository(FinanceServiceReadDbContext dbContext) : IUserCurrencyReadRepository
{
    public Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken) =>
        dbContext.UserCurrencies
            .Where(u => u.UserId == userId)
            .Select(u => new UserCurrencyDto(u.CurrencyId, u.Currency.Name, u.Currency.Rate))
            .ToArrayAsync(cancellationToken);
}
