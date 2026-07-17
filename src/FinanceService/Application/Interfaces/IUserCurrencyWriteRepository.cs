using Domain.Aggregates;

namespace Application.Interfaces;

public interface IUserCurrencyWriteRepository
{
    Task Add(UserCurrency userCurrency, CancellationToken cancellationToken);
    Task Remove(UserCurrency userCurrency, CancellationToken cancellationToken);
}
