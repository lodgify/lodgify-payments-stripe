namespace Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;

public interface IAccountHistoryRepository
{
    Task AddAsync(AccountHistory accountHistory, CancellationToken cancellationToken);
}