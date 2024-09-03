namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public sealed record CreateAccountSessionCommandResponse(string StripeAccountId, string ClientSecret);