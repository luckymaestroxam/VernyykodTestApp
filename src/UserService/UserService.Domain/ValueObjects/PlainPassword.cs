namespace UserService.Domain.ValueObjects;

public sealed record PlainPassword
{
    private const int MinLength = 8;
    private const int MaxLength = 64;

    private PlainPassword(string value) => Value = value;

    public string Value { get; }

    public static PlainPassword Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, "Пароль не может быть пустым.");
        if (value.Length < MinLength)
        {
            throw new ArgumentException($"Пароль должен содержать минимум {MinLength} символов.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Пароль должен содержать максимум {MaxLength} символа.");
        }

        if (value.Any(char.IsWhiteSpace))
        {
            throw new ArgumentException("Пароль не должен содержать пробелы.");
        }

        if (!value.Any(char.IsLetter))
        {
            throw new ArgumentException("Пароль должен содержать хотя бы одну букву.");
        }

        if (!value.Any(char.IsDigit))
        {
            throw new ArgumentException("Пароль должен содержать хотя бы одну цифру.");
        }

        return new PlainPassword(value);
    }
}
