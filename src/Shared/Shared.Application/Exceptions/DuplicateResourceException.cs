namespace Application.Exceptions;

public class DuplicateResourceException(string message, Exception innerException) : Exception(message, innerException);
