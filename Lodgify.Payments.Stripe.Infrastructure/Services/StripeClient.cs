using Stripe;

namespace Lodgify.Payments.Stripe.Infrastructure.Services;

public class StripeClient : Lodgify.Payments.Stripe.Application.Services.IStripeClient
{
    public async Task<Lodgify.Payments.Stripe.Domain.Accounts.Account> CreateAccount(string country, string email, string feePayer, string lossPayments, string dashboardType, CancellationToken cancellationToken = default)
    {
        StripeConfiguration.ApiKey = "";
        var options = new AccountCreateOptions
        {
            Country = country,
            Email = email,
            Controller = new AccountControllerOptions
            {
                Fees = new AccountControllerFeesOptions { Payer = feePayer },
                Losses = new AccountControllerLossesOptions { Payments = lossPayments },
                StripeDashboard = new AccountControllerStripeDashboardOptions
                {
                    Type = dashboardType,
                },
            },
        };
        var service = new AccountService();
        var stripeAccount = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return Domain.Accounts.Account.Create(
            stripeAccount.Id,
            stripeAccount.ExternalAccounts.Url,
            "",
            stripeAccount.Metadata["order_id"].ToString(),
            //CurrentlyDue = stripeAccoun
            //EventuallyDue = stripeAccount.EventuallyDue,
            //PastDue = stripeAccount.PastDue,
            //DisabledReason = stripeAccount.DisabledReason,
            stripeAccount.Type
        );
    }
}