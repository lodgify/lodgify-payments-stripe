﻿using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public sealed record CreateAccountSessionCommand(string StripeAccountId) : ICommand<CreateAccountSessionResponse>
{
    public LodgifyAccount Account { get; set; } = null!;
}