using Domain.Aggregates;

namespace Application.Interfaces;

public interface IUserReadRepository
{
    Task<User?> Get(string name, CancellationToken cancellationToken);
}
