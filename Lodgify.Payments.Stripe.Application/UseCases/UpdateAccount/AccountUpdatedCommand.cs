﻿using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;

public sealed record AccountUpdatedCommand(string AccountId, bool ChargesEnabled, bool DetailsSubmitted, string RawSourceEventData, string SourceEventId, DateTime CreatedAt) : ICommand;