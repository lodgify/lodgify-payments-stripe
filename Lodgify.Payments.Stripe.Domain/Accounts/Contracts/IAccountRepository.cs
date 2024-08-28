namespace Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

public interface IAccountRepository
{
    Task AddAccountAsync(Account account, CancellationToken cancellationToken = default);
    Task<int?> QueryAccountUserIdAsync(string stripeAccountId, CancellationToken cancellationToken = default);
}