namespace Lodgify.Payments.Stripe.Application.Services;

public interface IStripeClient
{
    Task<Lodgify.Payments.Stripe.Domain.Accounts.Account> CreateAccount(string country, string email, CancellationToken cancellationToken = default);
}