namespace Domain.ValueObjects;

public sealed record PasswordData
{
    private PasswordData(string value) => Value = value;
    public string Value { get; }

    public static PasswordData Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, "Хеш пароля не может быть пустым.");

        return new PasswordData(value);
    }
}
