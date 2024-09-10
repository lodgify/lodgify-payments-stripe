using System.Diagnostics.CodeAnalysis;

namespace Lodgify.Payments.Stripe.Api.Models.Requests;

[ExcludeFromCodeCoverage]
public class CreateAccountRequest
{
    public CreateAccountRequest(string country, string email)
    {
        Country = country;
        Email = email;
    }

    public string Country { get; }
    public string Email { get; }
}