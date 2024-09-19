using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public sealed record CreateAccountIdentifiedCommand(string Country, string Email) : IIdentifiedCommand<CreateAccountCommandResponse>
{
    public LodgifyAccount Account { get; set; } = null!;
}