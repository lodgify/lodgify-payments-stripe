using System.Text.Json;
using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.WebhookEvents;

public class WebhookEvent : Aggregate
{
    public Guid Id { get; init; }
    public string WebhookEventStripeId { get; init; }
    public JsonDocument RawEventData { get; init; }
    public DateTime CreatedAt { get; init; }
    
    public static WebhookEvent Create(string webhookEventStripeId, string rawEventData)
    {
        return new WebhookEvent
        {
            Id = Guid.NewGuid(),
            WebhookEventStripeId = webhookEventStripeId,
            RawEventData = JsonDocument.Parse(rawEventData),
            CreatedAt = DateTime.UtcNow
        };
    }
}