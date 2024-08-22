namespace Lodgify.Payments.Stripe.Infrastructure.Migrator.Contracts;

public interface IDatabaseMigrator
{
    Task<IEnumerable<string>> GetPendingMigrationsAsync(CancellationToken cancel);
    Task MigrateAsync(string target, CancellationToken cancel);
}