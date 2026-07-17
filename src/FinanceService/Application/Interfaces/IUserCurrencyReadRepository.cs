using Application.Models;
using Domain.Aggregates;

namespace Application.Interfaces;

public interface IUserCurrencyReadRepository
{
    Task<bool> Exists(UserCurrency userCurrency, CancellationToken cancellationToken);
    Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken);
}
