namespace Lodgify.Payments.Stripe.Server.UnitTests.Payloads;

public static class PayloadLoader
{
    public static string Load(string payload)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Payloads/{payload}");
        return File.ReadAllText(path);
    }
}