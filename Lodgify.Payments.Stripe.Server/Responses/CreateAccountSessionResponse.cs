namespace Lodgify.Payments.Stripe.Server.Responses;

public sealed record CreateAccountSessionResponse(string StripeAccountId, string ClientSecret);