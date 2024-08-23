# lodgify-payments-stripe
stripe payments

## Running migrations

Before attempting to run migrations, set the following environment variables (every time you switch to this project)

```powershell
$env:ASPNETCORE_ENVIRONMENT='Development'
$env:ConfigSettings='[ { "location":  "lodgify-payments-stripe/settings.json" } ]'
```

Then migrations can be added, the database can be updated/reverted, and migrations can be removed with the following commands:

```powershell
dotnet ef migrations add Initial --project Lodgify.Payments.Stripe.Infrastructure\Lodgify.Payments.Stripe.Infrastructure.csproj --startup-project Lodgify.Payments.Stripe.Server\Lodgify.Payments.Stripe.Server.csproj --context PaymentDbContext --output-dir Migrations --verbose
```
