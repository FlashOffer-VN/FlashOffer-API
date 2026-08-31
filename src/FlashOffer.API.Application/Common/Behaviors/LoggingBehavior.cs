using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FlashOffer.API.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("🟢 Processing {RequestName} at {Time}", requestName, DateTime.UtcNow);

        try
        {
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation(
                "✅ Completed {RequestName} in {Elapsed}ms at {Time}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                DateTime.UtcNow);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "❌ Failed {RequestName} in {Elapsed}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}