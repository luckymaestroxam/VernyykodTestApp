using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class RevokedTokenEntityConfiguration : IEntityTypeConfiguration<RevokedTokenEntity>
{
    public void Configure(EntityTypeBuilder<RevokedTokenEntity> entity)
    {
        entity.ToTable("revoked_token");
        entity.HasKey(e => e.Jti);
        entity.Property(e => e.Jti).HasColumnName("jti");
        entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
    }
}
