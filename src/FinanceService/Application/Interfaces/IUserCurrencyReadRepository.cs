using Application.Models;

namespace Application.Interfaces;

public interface IUserCurrencyReadRepository
{
    Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken);
}
