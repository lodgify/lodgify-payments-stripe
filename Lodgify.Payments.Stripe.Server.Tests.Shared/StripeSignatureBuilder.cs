namespace Lodgify.Payments.Stripe.Server.Tests.Shared;

public static class StripeSignatureBuilder
{
    private static string GenerateSigHeader(string payload, string key)
    {
        // Inspired by https://github.com/stripe/stripe-java/blob/master/src/test/java/com/stripe/net/WebhookTest.java
        var timestamp = GetTimeNow();
        var payloadToSign = string.Format("{0}.{1}", timestamp, payload);
        var signature = ComputeHmacSha256(key, payloadToSign);

        return string.Format("t={0},v1={1}", timestamp, signature);
    }

    private static long GetTimeNow()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    private static string ComputeHmacSha256(string key, string data)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    
    public static string GenerateSignature(string payload, string key)
    {
        return GenerateSigHeader(payload, key);
    }
}