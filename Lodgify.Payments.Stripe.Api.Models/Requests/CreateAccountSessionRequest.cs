using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Requests;

[ExcludeFromCodeCoverage]
public class CreateAccountSessionRequest
{
    public CreateAccountSessionRequest(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}