using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;

[CollectionDefinition(nameof(WireMockCollection))]
public class WireMockCollection : ICollectionFixture<CustomWebApplicationFactory<WireMockTestConfiguration>>;