using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserReadRepository(UserReadDbContext userReadDbContext) : IUserReadRepository
{
    public async Task<UserDto?> Get(string name, CancellationToken cancellationToken)
    {
        var userInDb = await userReadDbContext.Users.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

        return userInDb is null ? null : new UserDto(userInDb.Id, userInDb.Name, userInDb.Password);
    }
}
