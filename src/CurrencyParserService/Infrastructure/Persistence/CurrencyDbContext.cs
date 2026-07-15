using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyDto>(entity =>
        {
            entity.ToTable("currency");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Rate).HasColumnName("rate");
        });
    }
}
