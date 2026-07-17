using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserCurrencyWriteRepository(FinanceServiceWriteDbContext dbContext) : IUserCurrencyWriteRepository
{
    public async Task Add(UserCurrency userCurrency, CancellationToken cancellationToken) =>
        await dbContext.UserCurrencies.AddAsync(userCurrency.ToEntity(), cancellationToken);
}
