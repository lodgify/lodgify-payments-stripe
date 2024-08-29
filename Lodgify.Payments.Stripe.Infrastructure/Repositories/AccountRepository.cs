using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentDbContext _dbContext;

    public AccountRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddAccountAsync(Account account, CancellationToken cancellationToken)
    {
        await _dbContext.Account.AddAsync(account, cancellationToken);
    }

    public async Task<int?> QueryAccountUserIdAsync(string stripeAccountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Account
            .AsNoTracking()
            .Where(account => account.StripeAccountId == stripeAccountId)
            .Select(account => account.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}