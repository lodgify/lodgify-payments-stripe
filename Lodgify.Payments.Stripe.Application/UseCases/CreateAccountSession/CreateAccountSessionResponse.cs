namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public sealed record CreateAccountSessionResponse(string StripeAccountId, string ClientSecret);