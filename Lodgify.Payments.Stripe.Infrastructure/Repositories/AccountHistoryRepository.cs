using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class AccountHistoryRepository : IAccountHistoryRepository
{
    private readonly PaymentDbContext _dbContext;

    public AccountHistoryRepository(PaymentDbContext paymentDbContext)
    {
        _dbContext = paymentDbContext ?? throw new ArgumentNullException(nameof(paymentDbContext));
    }
    
    public async Task AddAsync(AccountHistory accountHistory, CancellationToken cancellationToken)
    {
        await _dbContext.AccountHistory.AddAsync(accountHistory, cancellationToken);
    }
}