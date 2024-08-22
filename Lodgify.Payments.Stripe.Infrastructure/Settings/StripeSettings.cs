namespace Lodgify.Payments.Stripe.Infrastructure.Settings;

public record StripeSettings
{
    public string ApiKey { get; init; }
}