using FinanceService.Infrastructure.Configurations;
using FinanceService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Persistence;

namespace FinanceService.Infrastructure.Persistence;

public class FinanceServiceWriteDbContext(DbContextOptions<FinanceServiceWriteDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<UserCurrencyEntity> UserCurrencies => Set<UserCurrencyEntity>();

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
        modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserCurrencyEntityConfiguration());
    }
}
