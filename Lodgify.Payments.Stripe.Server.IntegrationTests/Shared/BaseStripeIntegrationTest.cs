using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

public abstract class BaseStripeIntegrationTest : BaseIntegrationTest<StripeTestConfiguration>
{
    protected BaseStripeIntegrationTest(TestWebApplicationFactory<StripeTestConfiguration> factory) : base(factory)
    {
    }
}