using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;

public sealed record GetAccountsQuery : IQuery<IReadOnlyCollection<GetAccountsQueryResponse>>
{
    public LodgifyAccount Account { get; set; } = null!;
}