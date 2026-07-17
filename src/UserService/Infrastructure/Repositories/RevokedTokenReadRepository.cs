using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class RevokedTokenReadRepository(UserReadDbContext userReadDbContext) : IRevokedTokenReadRepository
{
    public Task<bool> Exists(Guid jti, CancellationToken cancellationToken) =>
        userReadDbContext.RevokedTokens.AnyAsync(r => r.Jti == jti, cancellationToken);
}
