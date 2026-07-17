using Shared.Domain.Aggregates;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Mappers;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class RevokedTokenWriteRepository(UserWriteDbContext userWriteDbContext) : IRevokedTokenWriteRepository
{
    public async Task Add(RevokedToken revokedToken, CancellationToken cancellationToken) =>
        await userWriteDbContext.RevokedTokens.AddAsync(revokedToken.ToEntity(), cancellationToken);
}
