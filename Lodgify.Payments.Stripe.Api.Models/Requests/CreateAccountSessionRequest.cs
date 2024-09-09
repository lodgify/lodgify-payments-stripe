namespace Lodgify.Payments.Stripe.Api.Models.Requests;

public class CreateAccountSessionRequest
{
    public CreateAccountSessionRequest(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}