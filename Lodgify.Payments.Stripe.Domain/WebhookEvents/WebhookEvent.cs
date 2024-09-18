using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Lodgify.Payments.Stripe.Domain.BuildingBlocks;
using UUIDNext;

namespace Lodgify.Payments.Stripe.Domain.WebhookEvents;

public class WebhookEvent : Aggregate
{
    public string WebhookEventStripeId { get; init; }
    public JsonDocument RawEventData { get; init; }
    public DateTime CreatedAt { get; init; }

    private WebhookEvent()
    {
    }

    internal WebhookEvent(Guid id) : base(id)
    {
    }

    public static WebhookEvent Create(string webhookEventStripeId, string rawEventData)
    {
        return new WebhookEvent(Uuid.NewDatabaseFriendly(Database.PostgreSql))
        {
            WebhookEventStripeId = webhookEventStripeId,
            RawEventData = JsonDocument.Parse(rawEventData),
            CreatedAt = DateTime.UtcNow
        };
    }
}