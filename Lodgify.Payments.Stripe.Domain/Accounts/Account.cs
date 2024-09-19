using Lodgify.Payments.Stripe.Domain.BuildingBlocks;
using UUIDNext;

namespace Lodgify.Payments.Stripe.Domain.Accounts;

public class Account : Aggregate
{
    public int UserId { get; init; }
    public string Email { get; init; } = null!;
    public string StripeAccountId { get; init; } = null!;
    public string Dashboard { get; init; } = null!;
    public string RequirementCollection { get; init; } = null!;
    public string Fees { get; init; } = null!;
    public string Losses { get; init; } = null!;
    public string ControllerType { get; init; } = null!;
    public bool ChargesEnabled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ChargesEnabledSetAt { get; private set; }
    public bool DetailsSubmitted { get; private set; }
    public DateTime DetailsSubmittedSetAt { get; private set; }


    private Account()
    {
    }

    internal Account(Guid id) : base(id)
    {
    }

    public static Account Create(int userId, string email, string stripeAccountId, string controllerType, string losses, string fees, string requirementCollection, string dashboard, bool chargesEnabled, bool detailsSubmitted, DateTime createdAt)
    {
        return new Account(Uuid.NewDatabaseFriendly(Database.PostgreSql))
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
            ChargesEnabledSetAt = createdAt,
            DetailsSubmitted = detailsSubmitted,
            DetailsSubmittedSetAt = createdAt,
            CreatedAt = createdAt
        };
    }

    public bool SetChargesEnabled(bool chargesEnabled, DateTime changeRequestedAt)
    {
        if (ChargesEnabledSetAt < changeRequestedAt && ChargesEnabled != chargesEnabled)
        {
            ChargesEnabled = chargesEnabled;
            ChargesEnabledSetAt = changeRequestedAt;
            return true;
        }
        
        return false;
    }
    
    public bool SetDetailsSubmitted(bool detailsSubmitted, DateTime changeRequestedAt)
    {
        if (DetailsSubmittedSetAt < changeRequestedAt && DetailsSubmitted != detailsSubmitted)
        {
            DetailsSubmitted = detailsSubmitted;
            DetailsSubmittedSetAt = changeRequestedAt;
            return true;
        }

        return false;
    }
}