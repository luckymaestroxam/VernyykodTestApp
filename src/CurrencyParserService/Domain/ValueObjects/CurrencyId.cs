using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record CurrencyId
{
    private static readonly Regex Pattern = new(@"^R\d{5}[A-Z]?$");

    public string Value { get; }

    private CurrencyId(string value) => Value = value;

    public static CurrencyId Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Идентификатор валюты не может быть пустым.", nameof(value));
        }

        var normalized = value.Trim();
        if (!Pattern.IsMatch(normalized))
        {
            throw new ArgumentException(
                $"Идентификатор валюты '{normalized}' имеет недопустимый формат. Ожидается R##### или R#####X.",
                nameof(value));
        }

        return new CurrencyId(normalized);
    }
}
