namespace FlashOffer.API.Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base() { }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException) { }

    public ValidationException(string field, string message)
        : base($"Validation failed for field '{field}': {message}") { }
}