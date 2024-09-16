using System;
using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.Accounts;

public class Account : Aggregate
{
    public int UserId { get; init; }
    public string Email { get; init; }
    public string StripeAccountId { get; init; }
    public string Dashboard { get; init; }
    public string RequirementCollection { get; init; }
    public string Fees { get; init; }
    public string Losses { get; init; }
    public string ControllerType { get; init; }
    public bool ChargesEnabled { get; private set; }
    public DateTime? ChargesEnabledAt { get; private set; }
    public bool DetailsSubmitted { get; private set; }
    public DateTime? DetailsSubmittedAt { get; private set; }


    private Account()
    {
    }

    internal Account(Guid id) : base(id)
    {
    }

    public static Account Create(int userId, string email, string stripeAccountId, string controllerType, string losses, string fees, string requirementCollection, string dashboard, bool chargesEnabled, bool detailsSubmitted)
    {
        var account = new Account(Guid.NewGuid())
        {
            UserId = userId,
            Email = email,
            StripeAccountId = stripeAccountId,
            ControllerType = controllerType,
            Losses = losses,
            Fees = fees,
            RequirementCollection = requirementCollection,
            Dashboard = dashboard
        };

        //verify this flag in case if stripe set chargesEnabled = true and detailsSubmitted = true direct after create account
        if (chargesEnabled)
        {
            account.SetChargesEnabled(chargesEnabled, DateTime.UtcNow);
        }

        if (detailsSubmitted)
        {
            account.SetDetailsSubmitted(detailsSubmitted, DateTime.UtcNow);
        }

        return account;
    }

    public bool SetChargesEnabled(bool chargesEnabled, DateTime changeRequestedAt)
    {
        if ((!ChargesEnabledAt.HasValue || ChargesEnabledAt.Value < changeRequestedAt) && ChargesEnabled != chargesEnabled)
        {
            ChargesEnabled = chargesEnabled;
            ChargesEnabledAt = changeRequestedAt;
            return true;
        }
        
        return false;
    }
    
    public bool SetDetailsSubmitted(bool detailsSubmitted, DateTime changeRequestedAt)
    {
        if ((!DetailsSubmittedAt.HasValue || DetailsSubmittedAt.Value < changeRequestedAt) && DetailsSubmitted != detailsSubmitted)
        {
            DetailsSubmitted = detailsSubmitted;
            DetailsSubmittedAt = changeRequestedAt;
            return true;
        }

        return false;
    }
}