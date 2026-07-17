using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Configurations;

namespace CurrencyParserService.Infrastructure.Persistence;

public class CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
}
