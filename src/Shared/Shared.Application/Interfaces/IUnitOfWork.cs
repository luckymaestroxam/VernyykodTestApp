namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken);
}
