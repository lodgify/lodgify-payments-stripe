using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

 public interface IQuery<out TResult> : IRequest<TResult>;