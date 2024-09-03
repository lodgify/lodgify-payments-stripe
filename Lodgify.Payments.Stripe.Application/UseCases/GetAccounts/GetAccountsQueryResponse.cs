namespace Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;

public record GetAccountsQueryResponse(List<AccountQueryResponse> Accounts);