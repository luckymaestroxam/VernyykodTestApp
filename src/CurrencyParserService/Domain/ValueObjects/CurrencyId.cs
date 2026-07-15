using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed partial record CurrencyId
{
    [GeneratedRegex(@"^R\d{5}[A-Z]?$", RegexOptions.CultureInvariant)]
    private static partial Regex CurrencyIdPattern();

    private static readonly Regex Pattern = CurrencyIdPattern();

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
