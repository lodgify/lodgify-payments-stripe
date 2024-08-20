using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentDbContext _dbContext;

    public AccountRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task AddAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        await _dbContext.Account.AddAsync(account, cancellationToken);
    }
}