using UserService.Domain.ValueObjects;

namespace UserService.Application.Interfaces;

public interface IPasswordHasher
{
    PasswordData Hash(PlainPassword plainPassword);
}
