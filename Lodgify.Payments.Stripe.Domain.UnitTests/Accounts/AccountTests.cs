using FluentAssertions;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Xunit;

namespace Lodgify.Payments.Stripe.Domain.UnitTests.Accounts;

public class AccountTests
{
    [Fact]
    public void Create_AccountWithChargesEnabledAndDetailsSubmitted_SetsPropertiesCorrectly()
    {
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", true, true, accountCreatedAt);

        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().Be(accountCreatedAt);
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().Be(accountCreatedAt);
    }

    [Fact]
    public void Create_AccountWithoutChargesEnabledAndDetailsSubmitted_SetsPropertiesCorrectly()
    {
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false, accountCreatedAt);

        account.ChargesEnabled.Should().BeFalse();
        account.ChargesEnabledSetAt.Should().Be(accountCreatedAt);
        account.DetailsSubmitted.Should().BeFalse();
        account.DetailsSubmittedSetAt.Should().Be(accountCreatedAt);
    }

    [Fact]
    public void SetChargesEnabled_ChangesChargesEnabledAndSetsTimestamp()
    {
        var accountCreatedAt = DateTime.UtcNow.AddMinutes(-1);
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false, accountCreatedAt);

        var changeRequestedAt = DateTime.UtcNow.AddMinutes(1);

        var result = account.SetChargesEnabled(true, changeRequestedAt);

        result.Should().BeTrue();
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().Be(changeRequestedAt);
        account.ChargesEnabledSetAt.Should().BeAfter(accountCreatedAt);
    }

    [Fact]
    public void SetChargesEnabled_DoesNotChangeIfTimestampIsOlder()
    {
        var accountCreatedAt = DateTime.UtcNow.AddMinutes(-1);
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", true, false, accountCreatedAt);

        var initialTimestamp = account.ChargesEnabledSetAt;
        var olderTimestamp = initialTimestamp.AddMinutes(-1);

        var result = account.SetChargesEnabled(false, olderTimestamp);

        result.Should().BeFalse();
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().Be(initialTimestamp);
    }

    [Fact]
    public void SetDetailsSubmitted_ChangesDetailsSubmittedAndSetsTimestamp()
    {
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, false, accountCreatedAt);
        var changeRequestedAt = DateTime.UtcNow;

        var result = account.SetDetailsSubmitted(true, changeRequestedAt);

        result.Should().BeTrue();
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().Be(changeRequestedAt);
    }

    [Fact]
    public void SetDetailsSubmitted_DoesNotChangeIfTimestampIsOlder()
    {
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", "acc_123", "controller", "losses", "fees", "requirements", "dashboard", false, true, accountCreatedAt);
        var initialTimestamp = account.DetailsSubmittedSetAt;
        var olderTimestamp = initialTimestamp.AddMinutes(-1);

        var result = account.SetDetailsSubmitted(false, olderTimestamp);

        result.Should().BeFalse();
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().Be(initialTimestamp);
    }
}