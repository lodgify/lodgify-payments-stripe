namespace Lodgify.Payments.Stripe.Api.Models.v1.Responses;

public class CreateAccountResponse
{
    public CreateAccountResponse(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}