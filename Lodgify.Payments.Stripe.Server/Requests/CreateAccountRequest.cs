namespace Lodgify.Payments.Stripe.Server.Requests;

public sealed record  CreateAccountRequest(string Country, string Email);