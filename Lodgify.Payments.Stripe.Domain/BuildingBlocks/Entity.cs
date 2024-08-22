namespace Lodgify.Payments.Stripe.Domain.BuildingBlocks;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id) => Id = id;

    protected Entity()
    {
    }


    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name}#[Identity={Id}]";

    public bool Equals(Entity other)
    {
        return other is not null && (ReferenceEquals(this, other) || Equals(Id, other.Id));
    }

    public override bool Equals(object obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((Entity)obj)));
    }

    public string GetId() => Id.ToString();

    public static bool operator ==(Entity a, Entity b)
    {
        return (a is null && b is null) || (a is not null && b is not null && a.Equals(b));
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }
}