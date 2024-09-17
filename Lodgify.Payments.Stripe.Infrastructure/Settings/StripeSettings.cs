using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Infrastructure.Settings;

[ExcludeFromCodeCoverage]
public record StripeSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string WebhookSecret { get; init; } = string.Empty;
}