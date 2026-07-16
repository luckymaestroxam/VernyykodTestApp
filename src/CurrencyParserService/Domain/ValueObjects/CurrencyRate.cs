namespace Domain.ValueObjects;

public sealed record CurrencyRate
{
    private CurrencyRate(decimal value) => Value = value;
    public decimal Value { get; }

    public static CurrencyRate Create(decimal value) =>
        value <= 0m
            ? throw new ArgumentException("Курс валюты должен быть больше нуля.", nameof(value))
            : new CurrencyRate(value);
}
