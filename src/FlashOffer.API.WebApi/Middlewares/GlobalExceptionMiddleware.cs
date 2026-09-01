using FlashOffer.API.WebApi.Responses;
using System.Net;
using System.Text.Json;
using FlashOffer.API.Shared.Exceptions;

namespace FlashOffer.API.WebApi.Middlewares;

public class GlobalExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<GlobalExceptionMiddleware> _logger;

	public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found - Path: {Path}, Method: {Method}",
                context.Request.Path, context.Request.Method);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            // Log đầy đủ thông tin
            _logger.LogError(ex, "❌ Unhandled exception - Path: {Path}, Method: {Method}, Query: {Query}, Body: {Body}",
                context.Request.Path,
                context.Request.Method,
                context.Request.QueryString.ToString(),
                await GetRequestBodyAsync(context.Request));

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == 0 || request.Body == null)
            return "N/A";

        request.EnableBuffering();
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var response = new ApiResponse<object>
		{
			Success = false,
			Message = "An error occurred while processing your request.",
			Errors = new List<string> { exception.Message },
			Timestamp = DateTime.UtcNow
		};

		await context.Response.WriteAsJsonAsync(response);
	}
}