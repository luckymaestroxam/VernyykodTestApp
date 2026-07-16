namespace Domain.ValueObjects;

public sealed record CurrencyName
{
    private const int MaxLength = 100;

    private CurrencyName(string value) => Value = value;

    public string Value { get; }

    public static CurrencyName Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Название валюты не может быть пустым.", nameof(value));
        }

        var normalized = value.Trim();

        return normalized.Length > MaxLength
            ? throw new ArgumentException($"Название валюты превышает допустимую длину ({MaxLength}).", nameof(value))
            : new CurrencyName(normalized);
    }
}
