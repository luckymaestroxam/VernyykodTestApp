using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IPasswordVerifier
{
    bool Matches(PlainPassword plainPassword, PasswordData passwordData);
}
