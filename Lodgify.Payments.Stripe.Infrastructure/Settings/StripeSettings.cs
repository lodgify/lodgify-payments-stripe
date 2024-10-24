namespace Lodgify.Payments.Stripe.Infrastructure.Settings;

public record StripeSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string WebhookSecret { get; init; } = string.Empty;
    public string ApiBase { get; init; } = string.Empty;
}