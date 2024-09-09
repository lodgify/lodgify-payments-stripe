using System.Text.Json.Serialization;
using Lodgify.Config.Providers;
using Lodgify.Config.Providers.Json;
using Lodgify.Extensions.AspNetCore;
using Lodgify.Extensions.AspNetCore.Mvc;
using Lodgify.Payments.Stripe.Application;
using Lodgify.Payments.Stripe.Infrastructure;
using Lodgify.Payments.Stripe.Server.HealthChecks;
using Lodgify.Payments.Stripe.Server.Middlewares;
using Mapster;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

namespace Lodgify.Payments.Stripe.Server.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IHostEnvironment hostEnvironment)
    {
        builder.Configuration.AddJsonLodgifyConfiguration(ConfigurationFileType.Json);
        builder.Configuration.AddJsonFile("appsettings.json");
        if (builder.Environment.IsDevelopment())
            builder.Configuration.AddJsonFile("appsettings.Development.json", true);

        builder.Services.AddMapster();
        
        builder.Services.AddApplication();

        builder.Services.AddTransient<CorrelationIdMiddleware>();
        builder.Services.AddTransient<UserScopeMiddleware>();
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();

        builder.Services.AddLodgifyAuthentication(
            builder.Configuration.GetValue<string>("identity:baseUrl")!,
            builder.Configuration);
        builder.Services.AddLodgifyAuthorization();

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("lodgify-payments-stripe"))
            .WithTracing(builder => builder
                .AddSource("lodgify-payments-stripe")
                .AddAspNetCoreInstrumentation()
                .SetErrorStatusOnException()
                .ConfigureResource(resource =>
                    resource.AddService(
                        serviceName: "lodgify-payments-stripe",
                        serviceVersion: "1")));

        builder.Services
            .AddHealthChecks()
            .AddCheck<MigratorHealthCheck>(nameof(MigratorHealthCheck))
            .AddCheck<OkHealthCheck>(nameof(OkHealthCheck));

        builder.Services
            .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(
            opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Lodgify payments Stripe API", Version = "v1" });
                opt.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
            });


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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpMetrics();
        app.UseMetricServer(settings => settings.EnableOpenMetrics = false);

        // app.UseLodgifyNLogMiddlewares();
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