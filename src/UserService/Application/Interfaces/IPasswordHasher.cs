using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IPasswordHasher
{
    PasswordData Hash(PlainPassword plainPassword);
}
