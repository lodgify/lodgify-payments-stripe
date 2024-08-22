using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public sealed record CreateAccountSessionCommand(string StripeAccountId) : ICommand<CreateAccountSessionResponse>;