using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface IIdentifiedCommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : IIdentifiedCommand;

public interface IIdentifiedCommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IIdentifiedCommand<TResponse>;