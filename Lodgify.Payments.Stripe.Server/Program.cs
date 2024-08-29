using Lodgify.Architecture.AspNetCore.Sentry;
using Lodgify.Extensions.Logging.NLog;
using Lodgify.Payments.Stripe.Server.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration)
    .AddSentry();

builder.WebHost
    .UseLodgifyNLog()
    .UseLodgifySentry(ThisAssembly.Git.Sha);

var logger = LogManager.Setup().GetCurrentClassLogger();
try
{
    var app = builder
        .ConfigureServices(builder.Environment)
        .ConfigurePipeline();

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Api host terminated unexpectedly");
}
finally
{
    LogManager.Shutdown();
}