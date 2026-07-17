namespace Application.Exceptions;

public class RepositoryConflictException(string message, Exception innerException) : Exception(message, innerException);
