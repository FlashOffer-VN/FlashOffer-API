namespace FlashOffer-API.Shared.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

public class BusinessException : Exception
{
    public BusinessException() : base() { }
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}
