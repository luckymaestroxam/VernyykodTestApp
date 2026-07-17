namespace FinanceService.Application.RequestHandlers.AddUserCurrency;

public sealed record AddUserCurrencyRequest(Guid UserId, string CurrencyId);
