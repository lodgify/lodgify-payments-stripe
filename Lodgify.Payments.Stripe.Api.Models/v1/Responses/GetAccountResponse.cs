using System.Collections.Generic;

namespace Lodgify.Payments.Stripe.Api.Models.v1.Responses;

public class GetAccountsResponse
{
    public GetAccountsResponse(List<AccountResponse> accounts)
    {
        Accounts = accounts;
    }

    public List<AccountResponse> Accounts { get; }
}