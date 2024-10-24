using Microsoft.Extensions.DependencyInjection;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;

public class AllowAnonymousFixture: BaseFixture
{
    protected override void AddLodgifyAuthorizationBase(IServiceCollection services)
    {
        //Do nothing and allow all requests
    }
}