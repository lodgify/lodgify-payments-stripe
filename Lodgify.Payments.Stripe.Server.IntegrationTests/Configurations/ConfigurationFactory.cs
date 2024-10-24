using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;

public class ConfigurationFactory
{
    internal const string ApiKey = "StripeSettings:ApiKey";
    internal const string ApiBase = "StripeSettings:ApiBase";
    internal const string WebhookSecret = "StripeSettings:WebhookSecret";
    internal const string IdentityBaseUrl = "Identity:BaseUrl";
    internal const string PostgresConnectionString = "ConnectionStrings:Postgres";
    
    public static IConfiguration GetDefaultConfiguration(string postgresConnectionString)
    {
        var integrationConfig = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.Tests.json")
            .Build();

        integrationConfig[PostgresConnectionString] = postgresConnectionString;
        integrationConfig[ApiBase] = "https://api.stripe.com";

        return integrationConfig;
    }

    public static IConfiguration GetWireMockConfiguration(string postgresConnectionString, string wireMockPublicUrl)
    {
        var integrationConfig = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
            .AddJsonFile("appsettings.Tests.json")
            .Build();

        integrationConfig[PostgresConnectionString] = postgresConnectionString;
        integrationConfig[IdentityBaseUrl] = wireMockPublicUrl;
        integrationConfig[ApiBase] = wireMockPublicUrl;

        return integrationConfig;
    }
}