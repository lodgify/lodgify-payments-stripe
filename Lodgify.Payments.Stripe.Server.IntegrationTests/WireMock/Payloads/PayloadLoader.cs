namespace Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Payloads;

public static class PayloadLoader
{
    public static string Load(string payload)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"WireMock/Payloads/{payload}");
        return File.ReadAllText(path);
    }
}