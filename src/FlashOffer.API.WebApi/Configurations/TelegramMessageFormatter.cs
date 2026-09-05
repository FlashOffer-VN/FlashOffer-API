using Serilog.Events;
using Serilog.Sinks.TelegramBot;
using System.Net;
using System.Text;

namespace FlashOffer.API.WebApi.Configurations;

/// <summary>
/// Custom message builder cho Telegram sink — định dạng đẹp, chia trường dễ đọc.
/// Dùng ParseMode.HTML để safe-escape toàn bộ nội dung (tránh lỗi Markdown bởi ký tự đặc biệt).
/// </summary>
public static class TelegramMessageFormatter
{
    private const int MaxMessageLength = 3800; // Telegram hard limit là 4096, để dư an toàn

    public static TelegramMessage Build(LogEvent logEvent)
    {
        var sb = new StringBuilder();

        Append(sb, BuildHeader(logEvent));
        Append(sb, BuildMessage(logEvent));
        Append(sb, BuildException(logEvent));
        Append(sb, BuildStackTrace(logEvent));
        Append(sb, BuildSource(logEvent));
        Append(sb, BuildProperties(logEvent));

        var text = sb.ToString().Trim();
        return new TelegramMessage(text, disableNotification: false);
    }

    private static StringBuilder BuildHeader(LogEvent logEvent)
    {
        var levelLabel = logEvent.Level.ToString().ToUpperInvariant();
        var emoji = logEvent.Level switch
        {
            LogEventLevel.Fatal => "⚫",
            LogEventLevel.Error => "🔴",
            LogEventLevel.Warning => "🟠",
            _ => "🔵"
        };

        var section = new StringBuilder();
        section.AppendLine($"<b>{emoji} {levelLabel} — FLASHOFFER.API</b>");
        section.AppendLine();
        section.AppendLine($"🕐 <code>{logEvent.Timestamp:yyyy-MM-dd HH:mm:ss 'UTC'}</code>");

        var environment = GetString(logEvent, "Environment");
        if (environment != null)
            section.AppendLine($"🌍 <code>{Html(environment)}</code>");

        var requestLine = BuildRequestLine(logEvent);
        if (requestLine != null)
            section.AppendLine(requestLine);

        return section;
    }

    private static string? BuildRequestLine(LogEvent logEvent)
    {
        var method = GetString(logEvent, "RequestMethod");
        var path = GetString(logEvent, "RequestPath");
        if (method == null && path == null)
            return null;

        var status = GetString(logEvent, "StatusCode");
        var line = "🔗 ";
        if (method != null)
            line += $"{Html(method)} ";
        if (path != null)
            line += $"<code>{Html(path)}</code>";
        if (!string.IsNullOrEmpty(status))
            line += $" → <b>{Html(status)}</b>";

        return line;
    }

    private static StringBuilder BuildMessage(LogEvent logEvent)
    {
        var section = new StringBuilder();
        section.AppendLine("<b>📌 MESSAGE</b>");
        section.AppendLine($"<pre>{Html(Cut(logEvent.RenderMessage(), 500))}</pre>");
        return section;
    }

    private static StringBuilder? BuildException(LogEvent logEvent)
    {
        if (logEvent.Exception == null)
            return null;

        var ex = logEvent.Exception;
        var section = new StringBuilder();
        section.AppendLine($"<b>🧨 {Html(ex.GetType().FullName ?? ex.GetType().Name)}</b>");
        if (!string.IsNullOrWhiteSpace(ex.Message))
            section.AppendLine($"💬 {Html(Cut(ex.Message, 400))}");
        return section;
    }

    private static StringBuilder? BuildStackTrace(LogEvent logEvent)
    {
        if (logEvent.Exception == null)
            return null;

        // Ưu tiên StackTrace thuần; fallback sang Exception.ToString() (bao gồm message + inner)
        var stackTrace = logEvent.Exception.StackTrace;
        if (string.IsNullOrWhiteSpace(stackTrace))
            stackTrace = logEvent.Exception.ToString();

        if (string.IsNullOrWhiteSpace(stackTrace))
            return null;

        var section = new StringBuilder();
        section.AppendLine("<b>🛠 STACK TRACE</b>");
        section.AppendLine($"<pre>{Html(Cut(stackTrace, 1400))}</pre>");
        return section;
    }

    private static StringBuilder? BuildSource(LogEvent logEvent)
    {
        var source = GetString(logEvent, "SourceContext");
        if (source == null)
            return null;

        var section = new StringBuilder();
        section.AppendLine($"📍 <code>{Html(source)}</code>");
        return section;
    }

    private static StringBuilder? BuildProperties(LogEvent logEvent)
    {
        var props = logEvent.Properties
            .Where(kv => kv.Key is not ("Environment" or "SourceContext" or "Application"
                or "RequestId" or "RequestPath" or "RequestMethod" or "StatusCode" or "Elapsed"))
            .Take(6)
            .ToList();

        if (props.Count == 0)
            return null;

        var section = new StringBuilder();
        section.AppendLine("<b>📋 PROPERTIES</b>");
        foreach (var p in props)
            section.AppendLine($"• <code>{Html(p.Key)}</code>: {Html(Cut(p.Value.ToString(), 200))}");
        return section;
    }

    /// <summary>
    /// Append section vào message nếu còn đủ chỗ — giữ toàn bộ section không bị cắt giữa
    /// (đảm bảo HTML luôn hợp lệ).
    /// </summary>
    private static void Append(StringBuilder sb, StringBuilder? section)
    {
        if (section == null || section.Length == 0)
            return;

        if (sb.Length + section.Length > MaxMessageLength)
            return;

        sb.Append(section);
        sb.AppendLine();
    }

    private static string? GetString(LogEvent logEvent, string key)
        => logEvent.Properties.TryGetValue(key, out var value)
            ? value.ToString().Trim('"')
            : null;

    private static string Html(string text) => WebUtility.HtmlEncode(text);

    private static string Cut(string text, int max)
        => text.Length <= max ? text : text[..max] + "…";
}