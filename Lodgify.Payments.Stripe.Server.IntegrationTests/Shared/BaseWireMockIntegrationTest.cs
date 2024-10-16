using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using WireMock.Admin.Mappings;
using WireMock.Client;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

[Collection(nameof(WireMockCollection))]
public abstract class BaseWireMockIntegrationTest(BaseFixture fixture) : BaseIntegrationTest(fixture)
{
    private readonly IWireMockAdminApi _wireMockClient = fixture.WiremockClient;

    protected async Task UseMockMapping(MappingModel mappingModel)
    {
        await _wireMockClient.PostMappingAsync(mappingModel);
    }
}