using Domain.Aggregates;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class UserCurrencyMapper
{
    internal static UserCurrencyEntity ToEntity(this UserCurrency userCurrency) =>
        new(userCurrency.UserId, userCurrency.CurrencyId.Value);
}
