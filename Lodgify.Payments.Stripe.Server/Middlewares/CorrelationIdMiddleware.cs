using Lodgify.Authentication.Constants;
using Lodgify.CorrelationContext;

namespace Lodgify.Payments.Stripe.Server.Middlewares;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = string.Empty;
        var requestId = context.TraceIdentifier;

        try
        {
            if (context.Request.Headers.TryGetValue(CorrelationKeys.CorrelationIdForHeader,
                    out var correlationIdValues))
                correlationId = correlationIdValues.FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
                correlationId = requestId;

            CorrelationStorage.Set(CorrelationKeys.CorrelationIdForHeader, correlationId);
            CorrelationStorage.Set(CorrelationKeys.CorrelationIdForLogs, correlationId);
            CorrelationStorage.Set(CorrelationKeys.RequestIdForLogs, requestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Cannot set {CorrelationKeys.CorrelationIdForLogs}.");
        }

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(CorrelationKeys.CorrelationIdForHeader, correlationId);
            return Task.CompletedTask;
        });

        using (_logger.BeginScope(new Dictionary<string, object?>
               {
                   { CorrelationKeys.RequestIdForLogs, requestId },
                   { CorrelationKeys.CorrelationIdForLogs, correlationId }
               }))
        {
            await _next.Invoke(context);
        }
    }
}