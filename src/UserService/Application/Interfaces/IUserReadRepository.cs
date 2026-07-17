using UserService.Application.Models;

namespace UserService.Application.Interfaces;

public interface IUserReadRepository
{
    Task<UserDto?> Get(string name, CancellationToken cancellationToken);
}
