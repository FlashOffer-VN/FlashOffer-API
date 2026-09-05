namespace FlashOffer.API.Shared.Common.Helpers;

/// <summary>
/// Thông tin thiết bị parse từ User-Agent.
/// </summary>
public sealed class UserAgentInfo
{
    public string? OperatingSystem { get; init; }
    public string? BrowserName { get; init; }
    public string? DeviceType { get; init; }
}

/// <summary>
/// Parse User-Agent để xác định hệ điều hành, trình duyệt, loại thiết bị.
/// Không dùng thư viện ngoài — chỉ phù hợp mục đích hiển thị/thống kê audit log.
/// </summary>
public static class UserAgentParser
{
    public static UserAgentInfo Parse(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return new UserAgentInfo();

        var ua = userAgent;
        var lower = ua.ToLowerInvariant();

        return new UserAgentInfo
        {
            OperatingSystem = DetectOperatingSystem(lower),
            BrowserName = DetectBrowser(ua, lower),
            DeviceType = DetectDeviceType(lower)
        };
    }

    private static string? DetectOperatingSystem(string ua)
    {
        // Trật tự quan trọng: Android chứa "Linux", iPhone/iPad chứa "Mac OS X"
        if (ua.Contains("windows phone")) return "Windows Phone";
        if (ua.Contains("windows")) return "Windows";
        if (ua.Contains("android")) return "Android";
        if (ua.Contains("iphone") || ua.Contains("ipod")) return "iOS";
        if (ua.Contains("ipad")) return "iPadOS";
        if (ua.Contains("mac os x") || ua.Contains("macintosh")) return "macOS";
        if (ua.Contains("linux")) return "Linux";
        if (ua.Contains("cros")) return "Chrome OS";
        if (ua.Contains("curl") || ua.Contains("postman") || ua.Contains("httpclient"))
            return null;
        return null;
    }

    private static string? DetectBrowser(string ua, string lower)
    {
        // Edg phải check trước Chrome (Edge UA chứa "chrome")
        if (lower.Contains("edg/") || lower.Contains("edgios") || lower.Contains("edge/"))
            return "Microsoft Edge";
        if (lower.Contains("opr/") || lower.Contains("opera"))
            return "Opera";
        if (lower.Contains("fxios") || lower.Contains("firefox"))
            return "Firefox";
        if (lower.Contains("samsungbrowser"))
            return "Samsung Internet";
        if (lower.Contains("crios") || lower.Contains("chrome"))
            return "Chrome";
        if (lower.Contains("safari"))
            return "Safari";
        if (lower.Contains("curl"))
            return "curl";
        if (lower.Contains("postman"))
            return "Postman";
        if (lower.Contains("insomnia"))
            return "Insomnia";
        if (lower.Contains("microsoftteams"))
            return "Microsoft Teams";
        if (ua.Contains("FlashOffer"))
            return "FlashOffer App";
        return null;
    }

    private static string? DetectDeviceType(string ua)
    {
        // Trật tự: iPad check trước iPhone/Mobile (biến thể iPad có "Mobile")
        if (ua.Contains("ipad") || ua.Contains("tablet"))
            return "Tablet";
        if (ua.Contains("iphone") || ua.Contains("ipod")
            || ua.Contains("android") || ua.Contains("mobile"))
            return "Mobile";
        return "Desktop";
    }
}