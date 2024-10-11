using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

public abstract class StripeIntegration : BaseIntegrationTest<StripeTestConfiguration>
{
    protected StripeIntegration(CustomWebApplicationFactory<StripeTestConfiguration> factory) : base(factory)
    {
    }
}