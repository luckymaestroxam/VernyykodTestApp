using Domain.Aggregates;

namespace Application.Interfaces;

public interface ITokenService
{
    string GetToken(User user);
}
