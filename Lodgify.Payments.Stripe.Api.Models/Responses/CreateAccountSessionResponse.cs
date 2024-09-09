namespace Lodgify.Payments.Stripe.Api.Models.Responses;

public class CreateAccountSessionResponse
{
    public CreateAccountSessionResponse(string stripeAccountId, string clientSecret)
    {
        StripeAccountId = stripeAccountId;
        ClientSecret = clientSecret;
    }
    public string StripeAccountId { get; set; }
    public string ClientSecret { get; set; }
}