using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Lodgify.Payments.Stripe.Application;

public static class ApplicationDependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        return services;
    }
}