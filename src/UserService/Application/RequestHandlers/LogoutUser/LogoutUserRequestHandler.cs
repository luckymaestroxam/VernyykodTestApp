using Application.Exceptions;
using Application.Interfaces;
using Domain.Aggregates;

namespace Application.RequestHandlers.LogoutUser;

public class LogoutUserRequestHandler(
    ITokenService tokenService,
    IRevokedTokenWriteRepository revokedTokenWriteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LogoutUserRequest, LogoutUserResponse>
{
    public async Task<LogoutUserResponse> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new UnauthorizedAccessException("Токен отсутствует.");
        }

        var tokenInfo = tokenService.GetTokenInfo(request.Token);
        if (!tokenInfo.IsValid)
        {
            throw new UnauthorizedAccessException("Токен невалидный.");
        }

        var revokedToken = RevokedToken.Create(tokenInfo.Jti, DateTime.UtcNow);

        try
        {
            await revokedTokenWriteRepository.Add(revokedToken, cancellationToken);
            await unitOfWork.SaveChanges(cancellationToken);
        }
        catch (RepositoryConflictException ex)
        {
            throw new UserAlreadyLogoutException("Выход уже был произведен ранее.", ex);
        }

        return new LogoutUserResponse();
    }
}
