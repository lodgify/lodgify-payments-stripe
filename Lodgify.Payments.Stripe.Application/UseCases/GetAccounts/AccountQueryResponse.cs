namespace Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;

public record AccountQueryResponse(string StripeAccountId, bool ChargesEnabled, bool DetailsSubmitted);