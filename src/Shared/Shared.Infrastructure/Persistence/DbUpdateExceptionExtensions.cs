using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence;

public static class DbUpdateExceptionExtensions
{
    public static void ThrowIfConstraintViolation(this DbUpdateException exception)
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
