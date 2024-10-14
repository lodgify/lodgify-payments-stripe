using Lodgify.Payments.Stripe.Application.Transactions;

namespace Lodgify.Payments.Stripe.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly PaymentDbContext _dbContext;

    public UnitOfWork(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task CommitAsync(CancellationToken cancel)
    {
        await _dbContext.SaveChangesAsync(cancel);
    }
}