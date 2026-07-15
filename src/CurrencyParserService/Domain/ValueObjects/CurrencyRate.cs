namespace Domain.ValueObjects;

public sealed record CurrencyRate
{
    public decimal Value { get; }

    private CurrencyRate(decimal value) => Value = value;

    public static CurrencyRate Create(decimal value)
    {
        return value <= 0m
            ? throw new ArgumentException("Курс валюты должен быть больше нуля.", nameof(value)) 
            : new CurrencyRate(value);
    }
}
