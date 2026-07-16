using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RevokedTokenWriteRepository(UserWriteDbContext userWriteDbContext) : IRevokedTokenWriteRepository
{
    public async Task Add(RevokedToken revokedToken, CancellationToken cancellationToken) =>
        await userWriteDbContext.RevokedTokens.AddAsync(revokedToken.ToEntity(), cancellationToken);
}
