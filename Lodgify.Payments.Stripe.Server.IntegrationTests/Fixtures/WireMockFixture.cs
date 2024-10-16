using Microsoft.AspNetCore.Hosting;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;

public class WireMockFixture : BaseFixture
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseConfiguration(LoadWireMockConfiguration());
    }
}