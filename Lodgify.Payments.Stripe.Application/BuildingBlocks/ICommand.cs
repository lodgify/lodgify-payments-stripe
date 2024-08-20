using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest;