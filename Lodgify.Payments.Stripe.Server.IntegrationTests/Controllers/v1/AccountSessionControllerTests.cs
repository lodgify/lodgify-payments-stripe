using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Domain.AccountSessions;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Mappings;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1;

[Collection(nameof(WireMockCollection))]
public class AccountSessionControllerTests : BaseWireMockIntegrationTest
{
    private const string RequestBaseUrl = "api/v1/account-sessions";

    public AccountSessionControllerTests(WireMockFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Post_ShouldReturnOk_WhenAccountSessionCreated()
    {
        //Arrange
        const int accountId = 2;
        const string stripeAccountId = "acct_1Q0hEfFaeO1hhNHt";

        await Insert(PredefinedMocks.AccountMock.CreateWithStripeAccountId(accountId, stripeAccountId));
        await UseMockMapping(WiremockMappings.StripeMappings.CreateAccountSession);

        //Act
        var response = await PostAsync(accountId, RequestBaseUrl, new CreateAccountSessionRequest(stripeAccountId));

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var accountSessionResponse = await DeserializeResponse<CreateAccountSessionResponse>(response);
        accountSessionResponse.Should().NotBeNull();
        accountSessionResponse.ClientSecret.Should().NotBeNullOrEmpty();

        await AssertSessionAccountCreated(accountSessionResponse.StripeAccountId, accountSessionResponse.ClientSecret);
    }

    private async Task AssertSessionAccountCreated(string stripeAccountId, string clientSecret)
    {
        var accountSession = await GetFirstAsync<AccountSession>(x => x.StripeAccountId == stripeAccountId && x.ClientSecret == clientSecret);
        accountSession.Should().NotBeNull();
    }
}