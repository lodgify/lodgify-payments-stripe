namespace Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;

public interface IWebhookEventRepository
{
    Task AddAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken);
}