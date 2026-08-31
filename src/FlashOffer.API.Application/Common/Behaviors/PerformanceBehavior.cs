using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FlashOffer.API.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _timer;
    private const long THRESHOLD_MS = 500; // Cảnh báo khi request chạy quá 500ms

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        var requestName = typeof(TRequest).Name;

        if (elapsedMilliseconds > THRESHOLD_MS)
        {
            _logger.LogWarning(
                "⚠️ Slow Request: {RequestName} took {Elapsed}ms (Threshold: {Threshold}ms)",
                requestName,
                elapsedMilliseconds,
                THRESHOLD_MS);
        }
        else
        {
            _logger.LogDebug(
                "⚡ Request: {RequestName} completed in {Elapsed}ms",
                requestName,
                elapsedMilliseconds);
        }

        return response;
    }
}