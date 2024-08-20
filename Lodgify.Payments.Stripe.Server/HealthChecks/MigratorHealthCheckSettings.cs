using Lodgify.Configuration.Contracts;

namespace Lodgify.Payments.Stripe.Server.HealthChecks;

[ConfigurationSection(new string[] { "migratorHealthCheck" })]
public class MigratorHealthCheckSettings
{
    public int TimeoutSeconds { get; set; }
    public int CacheExpirySeconds { get; set; }
    public string? TargetMigration { get; set; }

    public TimeSpan TimeoutSpan => TimeSpan.FromSeconds(TimeoutSeconds);
    public TimeSpan CacheExpirySpan => TimeSpan.FromSeconds(CacheExpirySeconds);
}