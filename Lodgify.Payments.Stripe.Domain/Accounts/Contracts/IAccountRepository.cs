using Lodgify.Payments.Stripe.Domain.Accounts.EntityViews;

namespace Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

public interface IAccountRepository
{
    Task<Account?> GetByStripeIdAsync(string stripeAccountId, CancellationToken cancellationToken);
    Task AddAccountAsync(Account account, CancellationToken cancellationToken);
    Task<int?> QueryAccountUserIdAsync(string stripeAccountId, CancellationToken cancellationToken);
    Task<List<AccountView>> QueryUserAccountsAsync(int userId, CancellationToken cancellationToken);
}