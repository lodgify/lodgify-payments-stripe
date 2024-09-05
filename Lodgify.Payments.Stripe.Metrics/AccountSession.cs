using Prometheus;

namespace Lodgify.Payments.Stripe.Metrics;

public static partial class AppMetrics
{
    public static class AccountSession
    {
        public const string AccountSessionCreated = "created";
        public const string AccountSessionCreating = "creating";
        public const string AccountSessionFailed = "failed";

        public static readonly Counter CreateAccountSessionCount = Prometheus.Metrics
            .CreateCounter(
                MetricName("create_account_session"),
                "Number of account sessions.",
                new CounterConfiguration
                {
                    LabelNames = new[]
                    {
                        "result"
                    }
                });

        public static void Created()
            => CreateAccountSessionCount
                .WithLabels(AccountSessionCreated)
                .Inc();

        public static void Creating()
            => CreateAccountSessionCount
                .WithLabels(AccountSessionCreating)
                .Inc();

        public static void Failed()
            => CreateAccountSessionCount
                .WithLabels(AccountSessionFailed)
                .Inc();
    }
}