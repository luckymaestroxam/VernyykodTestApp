namespace UserService.Infrastructure.Entities;

public sealed record UserEntity(Guid Id, string Name, string Password);
