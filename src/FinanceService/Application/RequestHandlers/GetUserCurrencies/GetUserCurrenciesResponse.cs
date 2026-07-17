using Application.Models;

namespace Application.RequestHandlers.GetUserCurrencies;

public sealed record GetUserCurrenciesResponse(UserCurrencyDto[] UserCurrencies);
