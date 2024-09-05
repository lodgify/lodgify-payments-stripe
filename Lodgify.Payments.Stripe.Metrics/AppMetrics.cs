namespace Lodgify.Payments.Stripe.Metrics;

public static partial class AppMetrics
{
    private const string AppPrefix = "payments_stripe";
    
    private static string MetricName(string metricName)
        => $"{AppPrefix}_{metricName}_total";

}