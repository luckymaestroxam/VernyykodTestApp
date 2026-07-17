namespace FinanceService.Application.RequestHandlers.RemoveUserCurrency;

public sealed record RemoveUserCurrencyRequest(Guid UserId, string CurrencyId);
