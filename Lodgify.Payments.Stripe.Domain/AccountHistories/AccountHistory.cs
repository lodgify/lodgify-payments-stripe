using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.AccountHistories;

public class AccountHistory : Aggregate
{
    public Guid AccountId { get; init; }

    public string PropertyName { get; init; }

    public string PropertyValue { get; init; }

    public DateTime SetAt { get; set; }

    public string WebhookEventStripeId { get; set; }
    
    public static AccountHistory Create(Guid accountId, string propertyName, string propertyValue, string webhookEventStripeId)
    {
        return new AccountHistory
        {
            AccountId = accountId,
            PropertyName = propertyName,
            PropertyValue = propertyValue,
            SetAt = DateTime.UtcNow,
            WebhookEventStripeId = webhookEventStripeId
        };
    }
}