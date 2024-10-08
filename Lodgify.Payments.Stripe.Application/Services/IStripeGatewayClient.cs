namespace Lodgify.Payments.Stripe.Application.Services;

public interface IStripeGatewayClient
{
    Task<Domain.Accounts.Account> CreateAccountAsync(int userId, string country, string email, CancellationToken cancellationToken);
    Task<Domain.AccountSessions.AccountSession> CreateAccountSessionAsync(string stripeAccountId, CancellationToken cancellationToken);
}