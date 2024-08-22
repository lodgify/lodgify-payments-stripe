using Lodgify.Payments.Stripe.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using AccountSession = Lodgify.Payments.Stripe.Domain.AccountSessions.AccountSession;

namespace Lodgify.Payments.Stripe.Infrastructure.Services;

public class StripeClient : Lodgify.Payments.Stripe.Application.Services.IStripeClient
{
    public StripeClient(IOptions<StripeSettings> stripeSettings)
    {
        StripeConfiguration.ApiKey = stripeSettings.Value.ApiKey;
    }

    public async Task<Lodgify.Payments.Stripe.Domain.Accounts.Account> CreateAccount(string country, string email, CancellationToken cancellationToken = default)
    {
        var options = new AccountCreateOptions
        {
            Country = country,
            Email = email,
            Controller = new AccountControllerOptions
            {
                StripeDashboard = new AccountControllerStripeDashboardOptions
                {
                    Type = "none",
                },
            },
            Capabilities = new AccountCapabilitiesOptions
            {
                CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                Transfers = new AccountCapabilitiesTransfersOptions { Requested = true }
            }
        };
        var service = new AccountService();
        var stripeAccount = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return Domain.Accounts.Account.Create(
            email,
            stripeAccount.Id,
            stripeAccount.Controller.Type,
            stripeAccount.Controller.Losses.Payments,
            stripeAccount.Controller.Fees.Payer,
            stripeAccount.Controller.RequirementCollection,
            stripeAccount.Controller.StripeDashboard.Type
        );
    }

    public async Task<AccountSession> CreateAccountSession(string stripeAccountId, CancellationToken cancellationToken = default)
    {
        var options = new AccountSessionCreateOptions
        {
            Account = stripeAccountId,
            Components = new AccountSessionComponentsOptions
            {
                AccountOnboarding = new AccountSessionComponentsAccountOnboardingOptions
                {
                    Enabled = true,
                    Features = new AccountSessionComponentsAccountOnboardingFeaturesOptions
                    {
                        ExternalAccountCollection = true,
                    },
                },
            },
        };
        var service = new AccountSessionService();
        var stripeAccountSession = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return AccountSession.Create(stripeAccountId, stripeAccountSession.ClientSecret);
    }
}