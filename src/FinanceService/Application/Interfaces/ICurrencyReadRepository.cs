using Domain.ValueObjects;

namespace Application.Interfaces;

public interface ICurrencyReadRepository
{
    Task<bool> Exists(CurrencyId currencyId, CancellationToken cancellationToken);
}
