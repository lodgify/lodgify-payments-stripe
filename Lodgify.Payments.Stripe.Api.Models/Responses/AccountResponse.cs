namespace Lodgify.Payments.Stripe.Api.Models.Responses;

public class AccountResponse
{
    public AccountResponse(string stripeAccountId)
    {
        StripeAccountId = stripeAccountId;
    }

    public string StripeAccountId { get; }
}