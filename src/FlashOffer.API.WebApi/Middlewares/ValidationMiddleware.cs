using FluentValidation;
using FlashOffer.API.WebApi.Responses;

namespace FlashOffer.API.WebApi.Middlewares;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public ValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var parameter = endpoint.DisplayName;
            // Validation will be handled by FluentValidation automatically
            // when using ModelState or calling validator manually
        }

        await _next(context);
    }
}
