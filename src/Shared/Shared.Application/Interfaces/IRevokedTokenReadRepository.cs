namespace Shared.Application.Interfaces;

public interface IRevokedTokenReadRepository
{
    Task<bool> Exists(Guid jti, CancellationToken cancellationToken);
}
