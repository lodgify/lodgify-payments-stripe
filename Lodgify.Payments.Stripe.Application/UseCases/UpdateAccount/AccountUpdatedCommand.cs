using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;

public sealed record AccountUpdatedCommand(string AccountId, bool ChargesEnabled, bool DetailsSubmitted, DateTime Created) : ICommand
{
    public LodgifyAccount Account { get; set; }
}