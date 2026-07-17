using Application.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

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
