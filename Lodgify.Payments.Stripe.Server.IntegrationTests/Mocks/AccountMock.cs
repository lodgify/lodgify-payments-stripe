using Lodgify.Payments.Stripe.Domain.Accounts;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;

public static partial class PredefinedMocks
{
    public static class AccountMock
    {
        public static Account Create(int userId)
        {
            return Account.Create(userId, "test@example.com", "acct_123", "application", "application", "stripe", "collection", "none", false, false, DateTime.UtcNow);
        }
        public static Account CreateWithStripeAccountId(int userId, string stripeAccountId)
        {
            return Account.Create(userId, "test@example.com", stripeAccountId, "application", "application", "stripe", "collection", "none", false, false, DateTime.UtcNow);
        }
        
    }
}