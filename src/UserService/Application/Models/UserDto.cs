namespace Application.Models;

public sealed record UserDto(Guid Id, string Name, string PasswordHash);
