using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Client.Constans;

public class PaymentStripeConstans
{
    public const string HttpClientName = "LodgifyPaymentsStripe";
    internal const string MetricsPrefix = "lodgify_payments_stripe_client";
    internal const string PropertyOwnerIdHeaderName = "X-PropertyOwnerId";
    internal const string SubOwnerIdHeaderName = "X-SubOwnerId";
}