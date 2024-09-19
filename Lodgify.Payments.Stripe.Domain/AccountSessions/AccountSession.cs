using Lodgify.Payments.Stripe.Domain.BuildingBlocks;
using UUIDNext;

namespace Lodgify.Payments.Stripe.Domain.AccountSessions;

public class AccountSession : Aggregate
{
    public string StripeAccountId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;

    private AccountSession()
    {
    }

    internal AccountSession(Guid id) : base(id)
    {
    }

    public static AccountSession Create(string stripeAccountId, string clientSecret)
    {
        return new AccountSession(Uuid.NewDatabaseFriendly(Database.PostgreSql))
        {
            StripeAccountId = stripeAccountId,
            ClientSecret = clientSecret
        };
    }
}