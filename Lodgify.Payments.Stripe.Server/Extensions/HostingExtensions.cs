﻿using System.Text.Json.Serialization;
using Lodgify.Config.Providers;
using Lodgify.Config.Providers.Json;
using Lodgify.Extensions.AspNetCore.Mvc;
using Lodgify.Extensions.Logging.NLog;
using Lodgify.Payments.Stripe.Application;
using Lodgify.Payments.Stripe.Infrastructure;
using Lodgify.Payments.Stripe.Server.HealthChecks;
using Lodgify.Payments.Stripe.Server.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Lodgify.Payments.Stripe.Server.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IHostEnvironment hostEnvironment)
    {
        builder.Configuration.AddJsonLodgifyConfiguration(ConfigurationFileType.Json);
        builder.Configuration.AddJsonFile("appsettings.json");
        if (builder.Environment.IsDevelopment())
            builder.Configuration.AddJsonFile("appsettings.Development.json", true);

        builder.Services.AddApplication();

        builder.Services
            .AddHealthChecks()
            .AddCheck<MigratorHealthCheck>(nameof(MigratorHealthCheck))
            .AddCheck<OkHealthCheck>(nameof(OkHealthCheck));

        builder.Services
            .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructure(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseLodgifyNLogMiddlewares();
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<UserScopeMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseLodgifyHealthchecks();
        app.MapHealthChecks("/api/status/startup",
            new HealthCheckOptions
            {
                Predicate = x =>
                    x.Name == nameof(MigratorHealthCheck)
            });

        app.MapHealthChecks("/api/status/liveness",
            new HealthCheckOptions
            {
                Predicate = x =>
                    x.Name == nameof(OkHealthCheck)
            });
        app.MapHealthChecks("/api/status/readiness",
            new HealthCheckOptions
            {
                Predicate = x =>
                    x.Name == nameof(OkHealthCheck)
            });

        app.MapControllers();

        return app;
    }
}