using Domain.Aggregates;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class RevokedTokenMapper
{
    internal static RevokedTokenEntity ToEntity(this RevokedToken revokedToken) =>
        new(revokedToken.Jti, revokedToken.RevokedAt);
}
