using Application.Models;
using Domain.Aggregates;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class UserCurrencyMapper
{
    internal static UserCurrencyDto ToDto(this UserCurrencyEntity userCurrency) =>
        new(userCurrency.CurrencyId, userCurrency.Currency.Name, userCurrency.Currency.Rate);

    internal static UserCurrencyEntity ToEntity(this UserCurrency userCurrency) =>
        new(userCurrency.UserId, userCurrency.CurrencyId.Value);
}
