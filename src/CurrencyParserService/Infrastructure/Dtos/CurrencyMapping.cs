using Domain.Aggregates;

namespace Infrastructure.Dtos;

internal static class CurrencyMapping
{
    internal static CurrencyDto ToDto(this Currency currency) =>
        new(currency.Id.Value, currency.Name.Value, currency.Rate.Value);
}
