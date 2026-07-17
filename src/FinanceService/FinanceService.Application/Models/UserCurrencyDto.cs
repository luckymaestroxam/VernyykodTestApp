namespace FinanceService.Application.Models;

public sealed record UserCurrencyDto(string CurrencyId, string Name, decimal Rate);
