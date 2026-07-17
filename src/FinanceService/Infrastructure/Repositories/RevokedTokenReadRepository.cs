using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RevokedTokenReadRepository(FinanceServiceReadDbContext userReadDbContext) : IRevokedTokenReadRepository
{
    public Task<bool> Exists(Guid jti, CancellationToken cancellationToken) =>
        userReadDbContext.RevokedTokens.AnyAsync(r => r.Jti == jti, cancellationToken);
}
