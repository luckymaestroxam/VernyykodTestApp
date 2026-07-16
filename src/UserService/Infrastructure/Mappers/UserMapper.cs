using Domain.Aggregates;
using Domain.ValueObjects;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class UserMapper
{
    internal static UserEntity ToEntity(this User user) => new(user.Id, user.Name.Value, user.PasswordData.Value);

    internal static User ToUser(this UserEntity userEntity) =>
        User.FromStorage(userEntity.Id, UserName.FromStorage(userEntity.Name),
            PasswordData.FromStorage(userEntity.Password));
}
