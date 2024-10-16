using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;

public class ConfigurationFactory
{
    public static IConfiguration GetDefaultConfiguration(string postgresConnectionString)
    {
        var integrationConfig = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.Tests.json")
            .Build();

        integrationConfig["ConnectionStrings:Postgres"] = postgresConnectionString;
        integrationConfig["StripeSettings:ApiBase"] = "https://api.stripe.com";

        return integrationConfig;
    }

    public static IConfiguration GetWireMockConfiguration(string postgresConnectionString, string wireMockPublicUrl)
    {
        var integrationConfig = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.Tests.json")
            .Build();

        integrationConfig["ConnectionStrings:Postgres"] = postgresConnectionString;
        integrationConfig["Identity:BaseUrl"] = wireMockPublicUrl;
        integrationConfig["StripeSettings:ApiBase"] = wireMockPublicUrl;

        return integrationConfig;
    }
}