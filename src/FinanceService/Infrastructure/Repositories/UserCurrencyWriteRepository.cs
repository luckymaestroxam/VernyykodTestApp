using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCurrencyWriteRepository(FinanceServiceWriteDbContext dbContext) : IUserCurrencyWriteRepository
{
    public async Task Add(UserCurrency userCurrency, CancellationToken cancellationToken) =>
        await dbContext.UserCurrencies.AddAsync(userCurrency.ToEntity(), cancellationToken);

    public async Task Remove(UserCurrency userCurrency, CancellationToken cancellationToken)
    {
        var userEntity = await dbContext.UserCurrencies.FirstOrDefaultAsync(u =>
                u.CurrencyId == userCurrency.CurrencyId.Value && u.UserId == userCurrency.UserId,
            cancellationToken);
        if (userEntity != null)
        {
            dbContext.UserCurrencies.Remove(userEntity);
        }
    }
}
