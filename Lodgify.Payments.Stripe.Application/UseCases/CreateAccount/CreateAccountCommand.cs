using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public sealed record CreateAccountCommand(string Country, string Email) : ICommand<CreateAccountResponse>
{
    public LodgifyAccount Account { get; set; } = null!;
}