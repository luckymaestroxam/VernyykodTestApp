using UserService.Domain.Aggregates;

namespace UserService.Application.Interfaces;

public interface IUserWriteRepository
{
    Task Add(User user, CancellationToken cancellationToken);
}
