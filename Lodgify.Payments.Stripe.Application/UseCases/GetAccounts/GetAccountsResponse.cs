namespace Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;

public record GetAccountsResponse(List<string> StripeAccountIds);