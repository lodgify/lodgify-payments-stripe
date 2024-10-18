using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

[Collection(nameof(StripeCollection))]
public abstract class BaseStripeIntegrationTest(StripeFixture fixture) : BaseIntegrationTest(fixture)
{
    protected string StripeApiKey => fixture.Configuration[ConfigurationFactory.ApiKey]!;
    protected string StripeApiBase => fixture.Configuration[ConfigurationFactory.ApiBase]!;
}