namespace Application.Exceptions;

public class UserAlreadyLogoutException(string message, Exception innerException) : Exception(message, innerException);
