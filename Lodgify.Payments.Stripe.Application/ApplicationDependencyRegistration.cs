using System.Reflection;
using Lodgify.Payments.Stripe.Application.Auth.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Lodgify.Payments.Stripe.Application;

public static class ApplicationDependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SetUserBehavior<,>));
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        return services;
    }
}