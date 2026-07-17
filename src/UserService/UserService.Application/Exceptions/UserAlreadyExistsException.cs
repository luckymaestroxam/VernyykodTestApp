namespace UserService.Application.Exceptions;

public class UserAlreadyExistsException(string message, Exception innerException) : Exception(message, innerException);
