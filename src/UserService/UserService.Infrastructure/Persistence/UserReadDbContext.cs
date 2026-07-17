using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Entities;
using UserService.Infrastructure.Configurations;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure.Persistence;

public class UserReadDbContext(DbContextOptions<UserReadDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
    }
}
