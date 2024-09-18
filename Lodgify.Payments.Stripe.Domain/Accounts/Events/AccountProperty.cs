using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Domain.Accounts.Events;

public class AccountProperty
{
    public string Name { get; }
    public string Value { get; }
    
    public AccountProperty(string name, string value)
    {
        Name = name;
        Value = value;
    }
}