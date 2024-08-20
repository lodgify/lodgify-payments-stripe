using System.Security.Claims;

namespace Lodgify.Payments.Stripe.Server.Middlewares;

/// <summary>
/// User Tracing.
/// </summary>
/// <param name="next">request delegate.</param>
/// <param name="logger">logger.</param>
public class UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is { IsAuthenticated: true } && !string.IsNullOrEmpty(context.User.Identity.Name))
        {
            var user = context.User;
            var subjectId = user.Claims.First(c => c.Type == "sub").Value;
            var userName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            using (logger.BeginScope("User:{User}, SubjectId:{Subject}", userName, subjectId))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}