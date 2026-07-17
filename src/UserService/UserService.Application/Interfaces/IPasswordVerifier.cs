using UserService.Domain.ValueObjects;

namespace UserService.Application.Interfaces;

public interface IPasswordVerifier
{
    bool Matches(PlainPassword plainPassword, PasswordData passwordData);
}
