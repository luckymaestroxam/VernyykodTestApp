using Shared.Domain.ValueObjects;

namespace FinanceService.Application.Interfaces;

public interface ICurrencyReadRepository
{
    Task<bool> Exists(CurrencyId currencyId, CancellationToken cancellationToken);
}
