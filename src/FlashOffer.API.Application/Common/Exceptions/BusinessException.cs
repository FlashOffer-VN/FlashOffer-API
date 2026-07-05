// Application/Common/Exceptions/BusinessException.cs
namespace FlashOffer.API.Application.Common.Exceptions;

public class BusinessException : Exception
{
    public BusinessException() { }

    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException) { }
}