using Domain.Aggregates;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

internal static class CurrencyMapper
{
    internal static CurrencyEntity ToEntity(this Currency currency) =>
        new(currency.Id.Value, currency.Name.Value, currency.Rate.Value);
}
