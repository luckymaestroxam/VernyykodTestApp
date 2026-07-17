using FinanceService.Domain.Aggregates;
using FinanceService.Infrastructure.Entities;

namespace FinanceService.Infrastructure.Mappers;

internal static class UserCurrencyMapper
{
    internal static UserCurrencyEntity ToEntity(this UserCurrency userCurrency) =>
        new(userCurrency.UserId, userCurrency.CurrencyId.Value);
}
