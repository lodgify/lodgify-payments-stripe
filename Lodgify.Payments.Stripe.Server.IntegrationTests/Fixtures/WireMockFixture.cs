using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Microsoft.AspNetCore.Hosting;
using WireMock.Client;
using WireMock.Net.Testcontainers;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;

public class WireMockFixture : BaseFixture
{
    private readonly WireMockContainer _wireMockContainer = new WireMockContainerBuilder()
        .Build();

    public IWireMockAdminApi WiremockClient { get; private set; }

    public override async Task InitializeAsync()
    {
        await _wireMockContainer.StartAsync();
        WiremockClient = _wireMockContainer.CreateWireMockAdminClient();

        await base.InitializeAsync();
    }

    public override async Task DisposeAsync()
    {
        await _wireMockContainer.StopAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseConfiguration(ConfigurationFactory.GetWireMockConfiguration(Configuration[ConfigurationFactory.PostgresConnectionString], _wireMockContainer.GetPublicUrl()));
    }
}