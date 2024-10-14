using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;

[CollectionDefinition(nameof(StripeCollection))]
public class StripeCollection : ICollectionFixture<TestWebApplicationFactory<StripeTestConfiguration>>;