using FinanceService.Infrastructure.Configurations;
using FinanceService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Entities;

namespace FinanceService.Infrastructure.Persistence;

public class FinanceServiceReadDbContext(DbContextOptions<FinanceServiceReadDbContext> options) : DbContext(options)
{
    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();
    public DbSet<UserCurrencyEntity> UserCurrencies => Set<UserCurrencyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserCurrencyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
    }
}
