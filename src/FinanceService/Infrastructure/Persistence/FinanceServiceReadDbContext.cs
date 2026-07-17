using Infrastructure.Configurations;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class FinanceServiceReadDbContext(DbContextOptions<FinanceServiceReadDbContext> options) : DbContext(options)
{
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
    }
}
