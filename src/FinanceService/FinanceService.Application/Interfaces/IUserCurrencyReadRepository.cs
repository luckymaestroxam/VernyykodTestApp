using FinanceService.Application.Models;

namespace FinanceService.Application.Interfaces;

public interface IUserCurrencyReadRepository
{
    Task<UserCurrencyDto[]> GetMany(Guid userId, CancellationToken cancellationToken);
}
