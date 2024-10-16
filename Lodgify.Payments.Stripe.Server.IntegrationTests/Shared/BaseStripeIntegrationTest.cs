using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

public abstract class BaseStripeIntegrationTest(StripeFixture fixture) : BaseIntegrationTest(fixture)
{
    private const string ApiKey = "StripeSettings:ApiKey";
    private const string ApiBase = "StripeSettings:ApiBase";
    
    protected string StripeApiKey => fixture.Configuration[ApiKey]!;
    protected string StripeApiBase => fixture.Configuration[ApiBase]!;
}