using FluentAssertions;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Payloads;
using Lodgify.Payments.Stripe.Server.Tests.Shared;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1.External;

public class WebhooksControllerTests : BaseStripeWebhookIntegrationTest
{
    private const string RequestBaseUrl = "api/v1/external/webhooks/stripe";

    public WebhooksControllerTests(AllowAnonymousFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Post_ShouldReturnOk_And_AccountShouldBeUpdated()
    {
        // Arrange
        const int accountId = 11;
        const string stripeAccounId = "acct_1Q3f4IFVr5qq4aSN"; //this account id should be the same as the one in the payload
        var account = PredefinedMocks.AccountMock.CreateWithStripeAccountId(accountId, stripeAccounId, DateTime.UtcNow.AddYears(-1));
        await Insert(account);

        var payload = PayloadLoader.Load(PayloadType.AccountUpdatedWebhook);
        var stripeSignature = StripeSignatureBuilder.GenerateSignature(payload, StripeWebhookSecret);

        // Act
        var response = await PostStripeWebhookAsync(RequestBaseUrl, stripeSignature, payload);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbAccount = await GetSingleOrDefaultAsync<Account>(x => x.StripeAccountId == stripeAccounId);
        dbAccount.StripeAccountId.Should().Be(stripeAccounId);
        dbAccount.ChargesEnabled.Should().BeTrue();
        dbAccount.DetailsSubmitted.Should().BeTrue();
    }
}