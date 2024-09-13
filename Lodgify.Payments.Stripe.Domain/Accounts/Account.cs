using Lodgify.Payments.Stripe.Domain.Accounts.Events;
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
    public bool DetailsSubmitted { get; private set; }


    private Account()
    {
    }

    internal Account(Guid id) : base(id)
    {
    }

    public static Account Create(int userId, string email, string stripeAccountId, string controllerType, string losses, string fees, string requirementCollection, string dashboard, bool chargesEnabled, bool detailsSubmitted)
    {
        return new Account(Guid.NewGuid())
        {
            UserId = userId,
            Email = email,
            StripeAccountId = stripeAccountId,
            ControllerType = controllerType,
            Losses = losses,
            Fees = fees,
            RequirementCollection = requirementCollection,
            Dashboard = dashboard,
            ChargesEnabled = chargesEnabled,
            DetailsSubmitted = detailsSubmitted
        };
    }

    public void Update(bool chargesEnabled, bool detailsSubmitted)
    {
        ChargesEnabled = chargesEnabled;
        DetailsSubmitted = detailsSubmitted;
    }
}