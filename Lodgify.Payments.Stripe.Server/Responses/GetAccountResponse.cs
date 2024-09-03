namespace Lodgify.Payments.Stripe.Server.Responses;

public record GetAccountsResponse(List<AccountResponse> Accounts);