using Shared.Domain.ValueObjects;

namespace Shared.Domain.Aggregates;

public sealed record Currency
{
    private Currency(CurrencyId id, CurrencyName name, CurrencyRate rate)
    {
        Id = id;
        Name = name;
        Rate = rate;
    }

    public CurrencyId Id { get; }
    public CurrencyName Name { get; }
    public CurrencyRate Rate { get; }

    public static Currency Create(CurrencyId id, CurrencyName name, CurrencyRate rate) => new(id, name, rate);
}
