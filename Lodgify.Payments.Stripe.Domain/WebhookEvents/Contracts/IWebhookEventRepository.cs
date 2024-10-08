﻿using System.Threading;
using System.Threading.Tasks;

namespace Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;

public interface IWebhookEventRepository
{
    Task AddAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string stripeEventId, CancellationToken cancellationToken);
}