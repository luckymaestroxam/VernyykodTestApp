namespace Application.RequestHandlers.GetUserCurrencies;

public sealed record GetUserCurrenciesResponse(UserCurrencyDto[] UserCurrencies);

public sealed record UserCurrencyDto(string CurrencyId, string Name, decimal Rate);
