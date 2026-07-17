namespace FinanceService.Application.Exceptions;

public class CurrencyNotExistsException(string message) : Exception(message);
