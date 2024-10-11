using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Mappings;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1;

[Collection(nameof(WireMockCollection))]
public class WireMockAccountControllerTests : WireMockIntegration
{
    private const string RequestBaseUrl = "api/v1/accounts";

    public WireMockAccountControllerTests(CustomWebApplicationFactory<WireMockTestConfiguration> factory) : base(factory)
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
        var account = await DbContext.Set<Account>().Where(x => x.StripeAccountId == stripeAccountId).FirstOrDefaultAsync();
        account.Should().NotBeNull();

        var accountHistories = await DbContext.Set<AccountHistory>().Where(x => x.AccountId == account.Id).ToListAsync();
        accountHistories.Should().HaveCount(2);
    }
}