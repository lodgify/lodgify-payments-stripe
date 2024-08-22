namespace Lodgify.Payments.Stripe.Application.Transactions;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancel);
}