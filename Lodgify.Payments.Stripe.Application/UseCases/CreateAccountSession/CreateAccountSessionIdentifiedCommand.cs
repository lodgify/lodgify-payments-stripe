using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public sealed record CreateAccountSessionIdentifiedCommand(string StripeAccountId) : IIdentifiedCommand<CreateAccountSessionCommandResponse>
{
    public LodgifyAccount Account { get; set; } = null!;
}