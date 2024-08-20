using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.Accounts;

public class Account : Aggregate
{
    public string StripeAccountId { get; init; }
    public string Object { get; init; }
    public string ExternalAccountUrl { get; init; }
    public string LoginLinkUrl { get; init; }
    public string OrderId { get; init; }
    // public List<string> CurrentlyDue { get; init; }
    // public List<string> EventuallyDue { get; init; }
    // public List<string> PastDue { get; init; }
    // public string DisabledReason { get; init; }
    public string Type { get; init; }


    private Account()
    {
    }

    internal Account(Guid id) : base(id)
    {
    }

    public static Account Create(string stripeAccountId, string externalAccountUrl, string loginLinkUrl, string orderId, string type)
    {
        return new Account()
        {
            Id = Guid.NewGuid(),
            StripeAccountId = stripeAccountId,
            Object = "account",
            ExternalAccountUrl = externalAccountUrl,
            LoginLinkUrl = loginLinkUrl,
            OrderId = orderId,
            Type = type

        };
    }
}