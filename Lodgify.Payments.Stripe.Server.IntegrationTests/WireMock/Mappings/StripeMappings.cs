using WireMock.Admin.Mappings;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Mappings;

public static partial class WiremockMappings
{
    public static class StripeMappings
    {
        public static readonly MappingModel CreateAccount = new()
        {
            Guid = Guid.NewGuid(),
            Request = new RequestModel
            {
                Methods = new[] { "POST" },
                Path = "//v1/accounts",
            },
            Response = new ResponseModel
            {
                StatusCode = 200,
                Body = MockFileReader.ReadFile("stripe_create_account_response.json")
            }
        };
        
        public static readonly MappingModel CreateAccountSession = new()
        {
            Guid = Guid.NewGuid(),
            Request = new RequestModel
            {
                Methods = new[] { "POST" },
                Path = "//v1/account_sessions",
            },
            Response = new ResponseModel
            {
                StatusCode = 200,
                Body = MockFileReader.ReadFile("stripe_create_account_session_response.json")
            }
        };
    }
}