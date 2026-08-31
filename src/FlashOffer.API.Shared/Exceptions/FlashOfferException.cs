namespace FlashOffer.API.Shared.Exceptions;

public class FlashOfferException : Exception
{
    public string StatusCode { get; }
    public object? AdditionalData { get; }

    public FlashOfferException(string statusCode, string message, object? additionalData = null)
        : base(message)
    {
        StatusCode = statusCode;
        AdditionalData = additionalData;
    }
}