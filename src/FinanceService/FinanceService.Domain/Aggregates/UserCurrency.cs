using Shared.Domain.ValueObjects;

namespace FinanceService.Domain.Aggregates;

public sealed record UserCurrency
{
    private UserCurrency(Guid userId, CurrencyId currencyId)
    {
        UserId = userId;
        CurrencyId = currencyId;
    }

    public Guid UserId { get; }
    public CurrencyId CurrencyId { get; }

    public static UserCurrency Create(Guid userId, CurrencyId currencyId) => new(userId, currencyId);
}
