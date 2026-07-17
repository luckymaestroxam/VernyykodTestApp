using Application.Exceptions;
using Application.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence;

public class UserWriteDbContext(DbContextOptions<UserWriteDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RevokedTokenEntity> RevokedTokens => Set<RevokedTokenEntity>();

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            ThrowIfConstraintViolation(ex);
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RevokedTokenEntityConfiguration());
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
