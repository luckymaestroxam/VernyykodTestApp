using FinanceService.Application.Models;

namespace FinanceService.Application.RequestHandlers.GetUserCurrencies;

public sealed record GetUserCurrenciesResponse(UserCurrencyDto[] UserCurrencies);
