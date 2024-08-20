namespace Lodgify.Payments.Stripe.Application.Services;

public interface IStripeClient
{
    Task<Lodgify.Payments.Stripe.Domain.Accounts.Account> CreateAccount(string country, string email, string feePayer, string lossPayments, string dashboardType, CancellationToken cancellationToken = default);
}