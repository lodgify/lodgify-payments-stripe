using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Responses;

[ExcludeFromCodeCoverage]
public class CreateAccountResponse
{
    public CreateAccountResponse(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}