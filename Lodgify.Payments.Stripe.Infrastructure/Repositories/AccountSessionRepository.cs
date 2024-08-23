using Lodgify.Payments.Stripe.Domain.AccountSessions;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class AccountSessionRepository : IAccountSessionRepository
{
    private readonly PaymentDbContext _dbContext;

    public AccountSessionRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task AddAccountAsync(AccountSession account, CancellationToken cancellationToken)
    {
        await _dbContext.AccountSession.AddAsync(account, cancellationToken);
    }
}