namespace Application.Interfaces;

public interface ITokenService
{
    string GetToken(Guid userId, string userName);
}
