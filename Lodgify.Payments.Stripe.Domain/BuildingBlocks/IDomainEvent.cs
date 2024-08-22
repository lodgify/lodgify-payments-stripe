namespace Lodgify.Payments.Stripe.Domain.BuildingBlocks;

public interface IDomainEvent
{
     public Guid Id { get; }
}