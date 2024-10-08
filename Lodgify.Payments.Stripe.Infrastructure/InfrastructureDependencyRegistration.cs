﻿using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;
using Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;
using Lodgify.Payments.Stripe.Infrastructure.Migrator;
using Lodgify.Payments.Stripe.Infrastructure.Migrator.Contracts;
using Lodgify.Payments.Stripe.Infrastructure.Repositories;
using Lodgify.Payments.Stripe.Infrastructure.Services;
using Lodgify.Payments.Stripe.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lodgify.Payments.Stripe.Infrastructure;

public static class InfrastructureDependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var contextBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        var connectionString = config.GetConnectionString("Postgres");
        contextBuilder.UseNpgsql(connectionString, o => o.SetPostgresVersion(13, 0));

        services.AddSingleton(contextBuilder.Options);
        services.AddDbContext<PaymentDbContext>(
            options => options.UseNpgsql(connectionString, o => o.SetPostgresVersion(13, 0)),
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Scoped);
        
        services.Configure<StripeSettings>(options => config.GetSection("StripeSettings").Bind(options));

        services.AddScoped<Lodgify.Payments.Stripe.Application.Services.IStripeGatewayClient, StripeGatewayClient>();
        services.AddScoped<IDatabaseMigrator, PaymentMigrator>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountSessionRepository, AccountSessionRepository>();
        services.AddScoped<IAccountHistoryRepository, AccountHistoryRepository>();
        services.AddScoped<IWebhookEventRepository, WebhookEventRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
}