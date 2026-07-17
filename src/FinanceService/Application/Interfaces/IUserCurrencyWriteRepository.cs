using Domain.Aggregates;

namespace Application.Interfaces;

public interface IUserCurrencyWriteRepository
{
    Task Add(UserCurrency userCurrency, CancellationToken cancellationToken);
}
