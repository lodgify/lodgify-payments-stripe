using System.Threading;
using System.Threading.Tasks;

namespace Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;

public interface IAccountSessionRepository
{
    Task AddAccountAsync(AccountSession account, CancellationToken cancellationToken);
}