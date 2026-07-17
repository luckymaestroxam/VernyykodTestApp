using Shared.Domain.Aggregates;
using Shared.Infrastructure.Entities;

namespace UserService.Infrastructure.Mappers;

internal static class RevokedTokenMapper
{
    internal static RevokedTokenEntity ToEntity(this RevokedToken revokedToken) =>
        new(revokedToken.Jti, revokedToken.RevokedAt);
}
