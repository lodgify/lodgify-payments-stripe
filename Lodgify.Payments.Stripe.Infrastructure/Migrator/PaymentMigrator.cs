using Lodgify.Payments.Stripe.Infrastructure.Migrator.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lodgify.Payments.Stripe.Infrastructure.Migrator;

internal class PaymentMigrator : IDatabaseMigrator
{
    private readonly PaymentDbContext _dbContext;

    public PaymentMigrator(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<string>> GetPendingMigrationsAsync(CancellationToken cancel)
    {
        return await _dbContext.Database.GetPendingMigrationsAsync(cancel);
    }

    public async Task MigrateAsync(string target, CancellationToken cancel)
    {
        var migrator = _dbContext
                           .GetInfrastructure()
                           .GetService(typeof(IMigrator)) as IMigrator
                       ?? throw new NotSupportedException($"Cannot resolve {typeof(IMigrator)}");

        await migrator.MigrateAsync(target, cancel);
    }
}