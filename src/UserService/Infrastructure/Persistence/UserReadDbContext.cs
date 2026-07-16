using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class UserReadDbContext(DbContextOptions<UserReadDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
        });

        modelBuilder.Entity<RevokedTokenEntity>(entity =>
        {
            entity.ToTable("revoked_token");
            entity.HasKey(e => e.Jti);
            entity.Property(e => e.Jti).HasColumnName("jti");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
        });
    }
}
