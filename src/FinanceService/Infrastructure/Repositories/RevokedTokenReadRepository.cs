using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RevokedTokenReadRepository(FinanceServiceReadDbContext dbContext) : IRevokedTokenReadRepository
{
    public Task<bool> Exists(Guid jti, CancellationToken cancellationToken) =>
        dbContext.RevokedTokens.AnyAsync(r => r.Jti == jti, cancellationToken);
}
