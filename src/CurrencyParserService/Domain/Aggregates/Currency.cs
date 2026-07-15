using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed record Currency
{
    public CurrencyId Id { get; }
    public CurrencyName Name { get; }
    public CurrencyRate Rate { get; }

    public static Currency Create(CurrencyId id, CurrencyName name, CurrencyRate rate)
    {
        return new Currency(id, name, rate);
    }

    private Currency(CurrencyId id, CurrencyName name, CurrencyRate rate)
    {
        Id = id;
        Name = name;
        Rate = rate;
    }
}
