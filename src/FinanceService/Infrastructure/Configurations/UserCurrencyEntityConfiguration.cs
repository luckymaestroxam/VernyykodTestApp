using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class UserCurrencyEntityConfiguration : IEntityTypeConfiguration<UserCurrencyEntity>
{
    public void Configure(EntityTypeBuilder<UserCurrencyEntity> entity)
    {
        entity.ToTable("user_currency");
        entity.HasKey(f => new { f.UserId, f.CurrencyId });
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
        entity.HasOne(f => f.Currency).WithMany().HasForeignKey(f => f.CurrencyId);
    }
}
