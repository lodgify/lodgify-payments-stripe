using Lodgify.Payments.Stripe.Domain.WebhookEvents;
using Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class WebhookEventRepository : IWebhookEventRepository
{
    private readonly PaymentDbContext _dbContext;

    public WebhookEventRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        await _dbContext.WebhookEvent.AddAsync(webhookEvent, cancellationToken);
    }

    public async Task<bool> Exists(string stripeEventId, CancellationToken cancellationToken)
    {
        return await _dbContext.WebhookEvent.AnyAsync(webhookEvent => webhookEvent.WebhookEventStripeId == stripeEventId, cancellationToken);
    }
}