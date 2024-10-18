using Lodgify.Payments.Stripe.Infrastructure;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;

public abstract class BaseFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:12")
        .WithDatabase("lodgify-payments-stripe-test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(8999, true)
        .Build();

    private IServiceScope _scope;
    internal IConfiguration Configuration;
    internal PaymentDbContext DbContext => _scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");
        builder.ConfigureLogging(logging => { logging.ClearProviders(); });

        Configuration = ConfigurationFactory.GetDefaultConfiguration(_postgresContainer.GetConnectionString());
        builder.UseConfiguration(Configuration);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PaymentDbContext>));

            services.AddDbContext<PaymentDbContext>(
                options => options.UseNpgsql(_postgresContainer.GetConnectionString()),
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped);

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("lodgify-jwt", AuthHelper.JwtBearerOptions);
        });
    }

    public virtual async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        _scope = Services.CreateScope();

        await MigrateDatabaseAsync<PaymentDbContext>();
        await SeedDatabaseAsync();
    }

    public new virtual async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        _scope.Dispose();
    }

    private async Task MigrateDatabaseAsync<TDbContext>() where TDbContext : DbContext
    {
        using var scope = Services.CreateScope();
        var dbContext = (scope.ServiceProvider.GetService(typeof(TDbContext)) as DbContext)!;

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(CancellationToken.None);

        var migrator = dbContext
            .GetInfrastructure()
            .GetService(typeof(IMigrator)) as IMigrator;

        await migrator!.MigrateAsync(pendingMigrations.Last(), CancellationToken.None);
    }

    private async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

        var transaction = await dbContext.Database.BeginTransactionAsync(CancellationToken.None);
        await transaction.CommitAsync(CancellationToken.None);

        await dbContext.SaveChangesAsync();
    }
}