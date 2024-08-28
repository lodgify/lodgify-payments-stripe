namespace Lodgify.Payments.Stripe.Application.Services;

public interface IStripeClient
{
    Task<Domain.Accounts.Account> CreateAccount(int userId, string country, string email, CancellationToken cancellationToken = default);
    Task<Domain.AccountSessions.AccountSession> CreateAccountSession(string stripeAccountId, CancellationToken cancellationToken = default);
}