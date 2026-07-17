using Application.Models;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class UserCurrencyMapper
{
    internal static UserCurrencyDto ToDto(this UserCurrencyEntity userCurrency) =>
        new(userCurrency.CurrencyId, userCurrency.Currency.Name, userCurrency.Currency.Rate);
}
