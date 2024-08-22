using Lodgify.Payments.Stripe.Infrastructure.Migrator.Contracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Lodgify.Payments.Stripe.Server.HealthChecks;

public class MigratorHealthCheck : IHealthCheck
{
    private bool _isHealthy;
    private readonly ILogger<MigratorHealthCheck> _logger;
    private readonly IDatabaseMigrator _migrator;
    private readonly string? _migrationTarget;

    public MigratorHealthCheck(
        IOptions<MigratorHealthCheckSettings> migratorSettingsOptions,
        ILogger<MigratorHealthCheck> logger,
        IDatabaseMigrator migrator)
    {
        var migratorSettings = migratorSettingsOptions.Value;
        _migrationTarget = migratorSettings.TargetMigration;
        _logger = logger;
        _migrator = migrator;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancel = default)
    {
        if (_isHealthy)
            return HealthCheckResult.Healthy("Done");

        var appliedMigrations = new List<string>();

        try
        {
            var target = _migrationTarget;

            if (string.IsNullOrEmpty(target))
            {
                var pendingMigrations = (await _migrator.GetPendingMigrationsAsync(cancel))
                    .ToArray();
                if (!pendingMigrations.Any())
                    return Healthy("No pending migrations");

                target = pendingMigrations.Last();
            }

            await _migrator.MigrateAsync(target, cancel);
            appliedMigrations.Add(target);
        }
        catch (Exception ex)
        {
            return Unhealthy(ex);
        }

        return Healthy(appliedMigrations.Count == 0
            ? "No pending migrations"
            : $"Migrated to {string.Join(",", appliedMigrations)}");
    }

    private HealthCheckResult Healthy(string message)
    {
        _isHealthy = true;
        _logger.LogInformation(message);
        return HealthCheckResult.Healthy(message);
    }

    private HealthCheckResult Unhealthy(Exception ex)
    {
        _logger.LogError(ex, "Migrator unhealthy");
        return HealthCheckResult.Unhealthy("Unexpected exception", ex);
    }
}