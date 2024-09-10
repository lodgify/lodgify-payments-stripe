using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Responses;

[ExcludeFromCodeCoverage]
public class AccountResponse
{
    public AccountResponse(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}