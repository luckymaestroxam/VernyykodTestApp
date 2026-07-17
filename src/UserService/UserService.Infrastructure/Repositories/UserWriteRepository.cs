using UserService.Application.Interfaces;
using UserService.Domain.Aggregates;
using UserService.Infrastructure.Mappers;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserWriteRepository(UserWriteDbContext userWriteDbContext) : IUserWriteRepository
{
    public async Task Add(User user, CancellationToken cancellationToken) =>
        await userWriteDbContext.Users.AddAsync(user.ToEntity(), cancellationToken);
}
