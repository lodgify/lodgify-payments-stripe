namespace Lodgify.Payments.Stripe.Api.Models.v1.Responses;

public class CreateAccountSessionResponse
{
    public CreateAccountSessionResponse(string stripeAccountId, string clientSecret)
    {
        StripeAccountId = stripeAccountId;
        ClientSecret = clientSecret;
    }

    public string StripeAccountId { get; }
    public string ClientSecret { get; }
}