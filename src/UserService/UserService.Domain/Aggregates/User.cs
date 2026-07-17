using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates;

public sealed record User
{
    private User(Guid id, UserName? name, PasswordData? passwordData)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name?.Value, "UserName не может быть пустым.");
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordData?.Value, "PasswordData не может быть пустым.");

        Id = id;
        Name = name;
        PasswordData = passwordData;
    }

    public Guid Id { get; }
    public UserName Name { get; }
    public PasswordData PasswordData { get; }

    public static User Create(UserName name, PasswordData passwordData) => new(Guid.NewGuid(), name, passwordData);
}
