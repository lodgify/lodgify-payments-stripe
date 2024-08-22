using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification;