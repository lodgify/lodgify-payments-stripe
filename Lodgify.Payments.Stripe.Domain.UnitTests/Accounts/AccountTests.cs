using FluentAssertions;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Xunit;

namespace Lodgify.Payments.Stripe.Domain.UnitTests.Accounts;

public class AccountTests
{
    [Fact]
    public void Create_AccountWithChargesEnabledAndDetailsSubmitted_SetsPropertiesCorrectly()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", true, true);

        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledAt.Should().NotBeNull();
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedAt.Should().NotBeNull();
    }

    [Fact]
    public void Create_AccountWithoutChargesEnabledAndDetailsSubmitted_SetsPropertiesCorrectly()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false);

        account.ChargesEnabled.Should().BeFalse();
        account.ChargesEnabledAt.Should().BeNull();
        account.DetailsSubmitted.Should().BeFalse();
        account.DetailsSubmittedAt.Should().BeNull();
    }

    [Fact]
    public void SetChargesEnabled_ChangesChargesEnabledAndSetsTimestamp()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false);
        var changeRequestedAt = DateTime.UtcNow;

        var result = account.SetChargesEnabled(true, changeRequestedAt);

        result.Should().BeTrue();
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledAt.Should().Be(changeRequestedAt);
    }

    [Fact]
    public void SetChargesEnabled_DoesNotChangeIfTimestampIsOlder()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", true, false);
        var initialTimestamp = account.ChargesEnabledAt;
        var olderTimestamp = initialTimestamp.Value.AddMinutes(-1);

        var result = account.SetChargesEnabled(false, olderTimestamp);

        result.Should().BeFalse();
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledAt.Should().Be(initialTimestamp);
    }

    [Fact]
    public void SetDetailsSubmitted_ChangesDetailsSubmittedAndSetsTimestamp()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false);
        var changeRequestedAt = DateTime.UtcNow;

        var result = account.SetDetailsSubmitted(true, changeRequestedAt);

        result.Should().BeTrue();
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedAt.Should().Be(changeRequestedAt);
    }

    [Fact]
    public void SetDetailsSubmitted_DoesNotChangeIfTimestampIsOlder()
    {
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, true);
        var initialTimestamp = account.DetailsSubmittedAt;
        var olderTimestamp = initialTimestamp.Value.AddMinutes(-1);

        var result = account.SetDetailsSubmitted(false, olderTimestamp);

        result.Should().BeFalse();
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedAt.Should().Be(initialTimestamp);
    }
}