namespace UserService.Domain.ValueObjects;

public sealed record UserName
{
    private UserName(string value) => Value = value;
    public string Value { get; }

    public static UserName Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, "Имя пользователя не может быть пустым.");

        return new UserName(value);
    }
}
