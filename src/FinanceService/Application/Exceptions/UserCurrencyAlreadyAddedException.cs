namespace Application.Exceptions;

public class UserCurrencyAlreadyAddedException(string message, Exception innerException)
    : Exception(message, innerException);
