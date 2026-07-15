using Domain.Aggregates;

namespace Infrastructure.Dtos;

public static class CurrencyMapping
{
    public static CurrencyDto ToDto(this Currency currency) =>
        new() { Id = currency.Id.Value, Name = currency.Name.Value, Rate = currency.Rate.Value };
}
