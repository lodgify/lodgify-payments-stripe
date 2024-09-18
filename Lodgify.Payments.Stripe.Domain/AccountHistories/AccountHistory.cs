using System;
using Lodgify.Payments.Stripe.Domain.BuildingBlocks;
using UUIDNext;

namespace Lodgify.Payments.Stripe.Domain.AccountHistories;

public class AccountHistory : Aggregate
{
    public Guid AccountId { get; init; }

    public string PropertyName { get; init; }

    public string PropertyValue { get; init; }

    public DateTime SetAt { get; set; }

    public string? WebhookEventStripeId { get; set; }

    private AccountHistory()
    {
    }

    internal AccountHistory(Guid id) : base(id)
    {
    }

    public static AccountHistory CreateInitialHistory(Guid accountId, string propertyName, string propertyValue, DateTime createdAt)
    {
        return new AccountHistory(Uuid.NewDatabaseFriendly(Database.PostgreSql))
        {
            AccountId = accountId,
            PropertyName = propertyName,
            PropertyValue = propertyValue,
            SetAt = createdAt
        };
    }

    public static AccountHistory Create(Guid accountId, string propertyName, string propertyValue, string webhookEventStripeId, DateTime createdAt)
    {
        return new AccountHistory(Uuid.NewDatabaseFriendly(Database.PostgreSql))
        {
            AccountId = accountId,
            PropertyName = propertyName,
            PropertyValue = propertyValue,
            SetAt = createdAt,
            WebhookEventStripeId = webhookEventStripeId
        };
    }
}