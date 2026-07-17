using Domain.Aggregates;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class UserMapper
{
    internal static UserEntity ToEntity(this User user) => new(user.Id, user.Name.Value, user.PasswordData.Value);
}
