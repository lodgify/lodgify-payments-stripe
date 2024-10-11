using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using WireMock.Admin.Mappings;
using WireMock.Client;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

public abstract class WireMockIntegration : BaseIntegrationTest<WireMockTestConfiguration>
{
    private readonly IWireMockAdminApi _wireMockClient;

    public WireMockIntegration(CustomWebApplicationFactory<WireMockTestConfiguration> factory) : base(factory)
    {
        _wireMockClient = factory.WiremockClient;
    }

    protected async Task UseMockMapping(MappingModel mappingModel)
    {
        await _wireMockClient.PostMappingAsync(mappingModel);
    }
}