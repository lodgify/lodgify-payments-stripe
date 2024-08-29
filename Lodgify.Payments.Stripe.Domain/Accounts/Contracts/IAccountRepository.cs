namespace Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

public interface IAccountRepository
{
    Task AddAccountAsync(Account account, CancellationToken cancellationToken);
    Task<int?> QueryAccountUserIdAsync(string stripeAccountId, CancellationToken cancellationToken);
}