using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Persistence;
using UserService.Infrastructure.Configurations;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure.Persistence;

public class UserWriteDbContext(DbContextOptions<UserWriteDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            ex.ThrowIfConstraintViolation();
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
    }
}
