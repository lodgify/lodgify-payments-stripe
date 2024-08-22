using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lodgify.Payments.Stripe.Server.HealthChecks;

public class OkHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancel = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}