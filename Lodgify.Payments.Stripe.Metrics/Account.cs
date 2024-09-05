using Prometheus;

namespace Lodgify.Payments.Stripe.Metrics;

public static partial class AppMetrics
{
    public static class Account
    {
        public const string AccountCreated = "created";
        public const string AccountCreating = "creating";
        public const string AccountCreatingFailed = "failed";

        public static readonly Counter CreateAccountCount = Prometheus.Metrics
            .CreateCounter(
                MetricName("create_account"),
                "Number of accounts.",
                new CounterConfiguration
                {
                    LabelNames = new[]
                    {
                        "result"
                    }
                });

        public static void Created()
            => CreateAccountCount
                .WithLabels(AccountCreated)
                .Inc();

        public static void Creating()
            => CreateAccountCount
                .WithLabels(AccountCreating)
                .Inc();
        
        public static void Failed()
            => CreateAccountCount
                .WithLabels(AccountCreatingFailed)
                .Inc();
    }
}