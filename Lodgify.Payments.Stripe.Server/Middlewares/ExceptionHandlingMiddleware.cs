using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Lodgify.Payments.Stripe.Server.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ProblemDetails? problemDetails = null;

        try
        {
            await next(context);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error");

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = $"Stripe error: {ex.StripeError.Code}",
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object?>()
                {
                    { "StackTrace", ex.StackTrace }
                }
            };
        }
        catch (BusinessRuleValidationException ex)
        {
            _logger.LogError(ex, "Business rule validation error");

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business rule validation error",
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object?>()
                {
                    { "StackTrace", ex.StackTrace }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Request processing error",
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object?>()
                {
                    { "StackTrace", ex.StackTrace }
                }
            };
        }

        if (problemDetails != null)
        {
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}