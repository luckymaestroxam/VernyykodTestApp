using FinanceService.Domain.Aggregates;

namespace FinanceService.Application.Interfaces;

public interface IUserCurrencyWriteRepository
{
    Task Add(UserCurrency userCurrency, CancellationToken cancellationToken);
    Task Remove(UserCurrency userCurrency, CancellationToken cancellationToken);
}
