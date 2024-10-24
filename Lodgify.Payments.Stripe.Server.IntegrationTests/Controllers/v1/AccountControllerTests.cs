using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Mappings;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1;

public class AccountControllerTests : BaseWireMockIntegrationTest
{
    private const string RequestBaseUrl = "api/v1/accounts";

    public AccountControllerTests(WireMockFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenAccountsAvailable()
    {
        //Arrange
        const int accountId = 1;
        var account = PredefinedMocks.AccountMock.Create(accountId);
        await Insert(account);

        //Act
        var response = await GetAsync(accountId, RequestBaseUrl);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Post_ShouldReturnOk_And_HistoryShouldBeCreated_WhenAccountsWasCreated()
    {
        //Arrange
        const int accountId = 1;
        await UseMockMapping(WiremockMappings.StripeMappings.CreateAccount);

        //Act
        var response = await PostAsync(accountId, RequestBaseUrl, new CreateAccountRequest(PredefinedMocks.CountryMock.Us, PredefinedMocks.EmailMock.User1));

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var accountResponse = await DeserializeResponse<CreateAccountResponse>(response);
        accountResponse.Should().NotBeNull();
        accountResponse.StripeAccountId.Should().NotBeNullOrEmpty();

        await AssertAccountHistoryCreated(accountResponse.StripeAccountId);
    }

    private async Task AssertAccountHistoryCreated(string stripeAccountId)
    {
        var account = await GetFirstAsync<Account>(x => x.StripeAccountId == stripeAccountId);
        account.Should().NotBeNull();

        var accountHistories = await GetListAsync<AccountHistory>(x => x.AccountId == account.Id);
        accountHistories.Should().HaveCount(2);
    }
}