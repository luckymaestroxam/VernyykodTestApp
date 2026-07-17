using Application.Exceptions;
using Application.Interfaces;
using Domain.Aggregates;

namespace Application.RequestHandlers.LogoutUser;

public class LogoutUserRequestHandler(
    IRevokedTokenWriteRepository revokedTokenWriteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LogoutUserRequest, LogoutUserResponse>
{
    public async Task<LogoutUserResponse> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var revokedToken = RevokedToken.Create(request.Jti, DateTime.UtcNow);

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
