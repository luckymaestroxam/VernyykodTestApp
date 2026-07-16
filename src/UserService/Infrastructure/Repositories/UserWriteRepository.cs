using Application.Interfaces;
using Domain.Aggregates;
using Infrastructure.Mappers;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserWriteRepository(UserWriteDbContext userWriteDbContext) : IUserWriteRepository
{
    public async Task Add(User user, CancellationToken cancellationToken) =>
        await userWriteDbContext.Users.AddAsync(user.ToEntity(), cancellationToken);
}
