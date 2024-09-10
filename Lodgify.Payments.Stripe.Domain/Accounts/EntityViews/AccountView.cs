namespace Lodgify.Payments.Stripe.Domain.Accounts.EntityViews;

public record AccountView(string StripeAccountId, bool ChargesEnabled, bool DetailsSubmitted);