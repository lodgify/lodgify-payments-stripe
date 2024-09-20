namespace Lodgify.Payments.Stripe.Api.Models.v1.Requests;

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