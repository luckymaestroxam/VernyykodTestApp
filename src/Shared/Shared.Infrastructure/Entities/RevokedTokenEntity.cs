namespace Infrastructure.Entities;

public sealed record RevokedTokenEntity(Guid Jti, DateTime RevokedAt);
