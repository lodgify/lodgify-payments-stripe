using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.AccountSessions;

public class AccountSession : Aggregate
{
    public string StripeAccountId { get; init; }
    public string ClientSecret { get; init; }

    private AccountSession()
    {
    }

    internal AccountSession(Guid id) : base(id)
    {
    }

    public static AccountSession Create(string stripeAccountId, string clientSecret)
    {
        return new AccountSession()
        {
            Id = Guid.NewGuid(),
            StripeAccountId = stripeAccountId,
            ClientSecret = clientSecret
        };
    }
}