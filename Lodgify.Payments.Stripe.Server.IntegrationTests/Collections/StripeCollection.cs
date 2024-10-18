using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;

[CollectionDefinition(nameof(StripeCollection))]
public class StripeCollection : ICollectionFixture<StripeFixture>;