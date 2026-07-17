using Shared.Domain.Aggregates;

namespace UserService.Application.Interfaces;

public interface IRevokedTokenWriteRepository
{
    Task Add(RevokedToken revokedToken, CancellationToken cancellationToken);
}
