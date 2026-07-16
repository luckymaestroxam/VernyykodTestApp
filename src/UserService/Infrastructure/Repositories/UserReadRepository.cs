using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserReadRepository(UserReadDbContext userReadDbContext) : IUserReadRepository
{
    public async Task<User?> Get(string name, CancellationToken cancellationToken)
    {
        var userInDb = await userReadDbContext.Users.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

        return userInDb?.ToUser();
    }
}
