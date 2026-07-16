using Domain.Aggregates;

namespace Application.Interfaces;

public interface IUserWriteRepository
{
    Task Add(User user, CancellationToken cancellationToken);
}
