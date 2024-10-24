using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

[Collection(nameof(StripeWebhookCollection))]
public abstract class BaseStripeWebhookIntegrationTest(AllowAnonymousFixture fixture) : BaseIntegrationTest(fixture)
{
    protected string StripeWebhookSecret => fixture.Configuration[ConfigurationFactory.WebhookSecret]!;
    
    protected Task<HttpResponseMessage> PostStripeWebhookAsync(string requestUrl, string signature, string content)
    {
        var client = Factory.CreateClient();
        
        client.DefaultRequestHeaders.Add("Stripe-Signature", signature);
        return client.PostAsync(requestUrl, new StringContent(content));
    }
}