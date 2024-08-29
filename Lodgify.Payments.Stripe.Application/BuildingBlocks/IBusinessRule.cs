namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface IBusinessRule
{
    string Message { get; }

    bool IsBroken();
}