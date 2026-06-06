namespace FlashOffer-API.Shared.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
}

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
    public static long ToUnixTimestamp(this DateTime dateTime) => ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
}
