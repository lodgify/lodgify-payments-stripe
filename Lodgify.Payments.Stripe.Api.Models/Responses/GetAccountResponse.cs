using System.Collections.Generic;

namespace Lodgify.Payments.Stripe.Api.Models.Responses;

public class GetAccountsResponse
{
    public GetAccountsResponse(List<AccountResponse> accounts)
    {
        Accounts = accounts;
    }

    public List<AccountResponse> Accounts { get; }
}