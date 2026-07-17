namespace Infrastructure.Entities;

public sealed record UserCurrencyEntity(Guid UserId, string CurrencyId)
{
    public CurrencyEntity Currency { get; init; } = null!;
}
