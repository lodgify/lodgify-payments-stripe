using Lodgify.Payments.Stripe.Domain.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Domain.Accounts.Events;

public sealed class AccountUpdatedEvent : IDomainEvent
{
    public Guid Id { get; }
    public Guid AccountId { get; init; }
    public List<AccountProperty> Properties { get; init; }
    public string RawSourceEventData { get; init; }
    public DateTime CreatedAt { get; }

    public AccountUpdatedEvent(Guid accountId, List<AccountProperty> properties, string rawSourceEventData)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        
        AccountId = accountId;
        Properties = properties;
        RawSourceEventData = rawSourceEventData;
    }
}