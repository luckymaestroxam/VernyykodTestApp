using Application.Models;

namespace Application.Interfaces;

public interface IUserReadRepository
{
    Task<UserDto?> Get(string name, CancellationToken cancellationToken);
}
