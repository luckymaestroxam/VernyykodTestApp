namespace Domain.ValueObjects;

public sealed record UserName
{
    private UserName(string value) => Value = value;
    public string Value { get; }

    internal static UserName FromStorage(string value) => new(value);

    public static UserName Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, "Имя пользователя не может быть пустым.");

        return new UserName(value);
    }
}
