using Infrastructure.Configurations;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class FinanceServiceReadDbContext(DbContextOptions<FinanceServiceReadDbContext> options) : DbContext(options)
{
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();
    public DbSet<UserCurrencyEntity> UserCurrencies => Set<UserCurrencyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserCurrencyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
    }
}
