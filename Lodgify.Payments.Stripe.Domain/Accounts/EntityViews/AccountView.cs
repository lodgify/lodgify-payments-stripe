using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Domain.Accounts.EntityViews;

[ExcludeFromCodeCoverage]
public record AccountView(string StripeAccountId, bool ChargesEnabled, bool DetailsSubmitted);