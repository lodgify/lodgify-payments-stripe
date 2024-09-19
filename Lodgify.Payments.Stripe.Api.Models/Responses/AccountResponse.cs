using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Responses;

public class AccountResponse
{
    public AccountResponse(string stripeAccountId, bool chargesEnabled, bool detailsSubmitted)
    {
        StripeAccountId = stripeAccountId;
        ChargesEnabled = chargesEnabled;
        DetailsSubmitted = detailsSubmitted;
    }

    public string StripeAccountId { get; }
    public bool ChargesEnabled { get; }
    public bool DetailsSubmitted { get; }
}