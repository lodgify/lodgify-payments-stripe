using System.Reflection;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock;

public static class MockFileReader
{
    public static string ReadFile(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceFile = assembly.GetManifestResourceNames()
            .FirstOrDefault(file => file.EndsWith(resourceName))!;
        
        using var stream = assembly.GetManifestResourceStream(resourceFile)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}