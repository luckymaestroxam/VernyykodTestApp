using Application.Exceptions;
using Application.Interfaces;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence;

public class UserWriteDbContext(DbContextOptions<UserWriteDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            return await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            ThrowIfConstraintViolation(ex);
            throw;
        }
    }

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

    private static void ThrowIfConstraintViolation(DbUpdateException exception)
    {
        for (var current = (Exception)exception; current is not null; current = current.InnerException)
        {
            if (current is PostgresException postgresException && IsUniqueOrForeignKeyViolation(postgresException))
            {
                throw new RepositoryConflictException("Нарушено ограничение уникальности или внешнего ключа.",
                    exception);
            }
        }
    }

    private static bool IsUniqueOrForeignKeyViolation(PostgresException exception) =>
        exception.SqlState is PostgresErrorCodes.UniqueViolation or PostgresErrorCodes.ForeignKeyViolation;
}
