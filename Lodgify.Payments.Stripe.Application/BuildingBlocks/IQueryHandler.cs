using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;