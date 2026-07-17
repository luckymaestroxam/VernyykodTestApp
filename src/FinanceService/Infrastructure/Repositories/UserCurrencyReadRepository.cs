using FinanceService.Application.Interfaces;
using FinanceService.Application.Models;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public class UserCurrencyReadRepository(FinanceServiceReadDbContext dbContext) : IUserCurrencyReadRepository
{
    public Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken) =>
        dbContext.UserCurrencies
            .Where(u => u.UserId == userId)
            .Select(u => new UserCurrencyDto(u.CurrencyId, u.Currency.Name, u.Currency.Rate))
            .ToArrayAsync(cancellationToken);
}
