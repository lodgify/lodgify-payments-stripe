﻿using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;

public class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, GetAccountsQueryResponse>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<GetAccountsQueryResponse> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.QueryUserAccountsAsync(query.Account.UserId, cancellationToken);
        return new GetAccountsQueryResponse(accounts.Select(account => new AccountQueryResponse(account.StripeAccountId, account.ChargesEnabled, account.DetailsSubmitted)).ToList());
    }
}