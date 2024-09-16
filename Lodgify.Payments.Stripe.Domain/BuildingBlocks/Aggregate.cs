using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lodgify.Payments.Stripe.Domain.BuildingBlocks;

public abstract class Aggregate : Entity
{
    protected Aggregate(Guid id) : base(id)
    {
    }

    protected Aggregate()
    {
    }

    private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void PublishEvent(IDomainEvent @event)
    {
        _domainEvents.Enqueue(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}