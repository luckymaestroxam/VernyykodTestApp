namespace Application.Exceptions;

public class UserCurrencyAlreadyAddedException : Exception
{
    public UserCurrencyAlreadyAddedException(string message) : base(message)
    {
    }

    public UserCurrencyAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
