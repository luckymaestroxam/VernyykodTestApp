using Domain.Aggregates;

namespace Application.Interfaces;

public interface IRevokedTokenWriteRepository
{
    Task Add(RevokedToken revokedToken, CancellationToken cancellationToken);
}
