using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Responses;

[ExcludeFromCodeCoverage]
public class GetAccountsResponse
{
    public GetAccountsResponse(List<AccountResponse> accounts)
    {
        Accounts = accounts;
    }

    public List<AccountResponse> Accounts { get; }
}