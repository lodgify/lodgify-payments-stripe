using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Xunit;
using Account = Lodgify.Payments.Stripe.Domain.Accounts.Account;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1;

[Collection(nameof(StripeCollection))]
public class StripeAccountControllerTests : BaseStripeIntegrationTest
{
    private const string RequestBaseUrl = "api/v1/accounts";

    public StripeAccountControllerTests(StripeFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Post_ShouldReturnOk_And_HistoryShouldBeCreated_WhenAccountsWasCreated()
    {
        //Arrange
        const int accountId = 3;

        //Act
        var createAccountResponse = await PostAsync(accountId, RequestBaseUrl, new CreateAccountRequest(PredefinedMocks.CountryMock.Us, PredefinedMocks.EmailMock.User1));

        // Assert
        createAccountResponse.IsSuccessStatusCode.Should().BeTrue();

        var accountResponse = await DeserializeResponse<CreateAccountResponse>(createAccountResponse);
        accountResponse.Should().NotBeNull();
        accountResponse.StripeAccountId.Should().NotBeNullOrEmpty();

        await AssertAccountExistsInStripe(accountResponse.StripeAccountId);
        await AssertAccountCreated(accountResponse.StripeAccountId);
        await AssertAccountHistoryCreated(accountResponse.StripeAccountId);
    }

    private async Task AssertAccountExistsInStripe(string stripeAccountId)
    {
        var service = new AccountService(new StripeClient(apiBase: StripeApiBase, apiKey: StripeApiKey));
        var account = await service.GetAsync(stripeAccountId);
        account.Should().NotBeNull();

        await service.DeleteAsync(stripeAccountId);
    }

    private async Task AssertAccountCreated(string stripeAccountId)
    {
        var account = await DbContext.Set<Account>().SingleOrDefaultAsync(x => x.StripeAccountId == stripeAccountId);
        account.Should().NotBeNull();
    }

    private async Task AssertAccountHistoryCreated(string stripeAccountId)
    {
        var account = await DbContext.Set<Account>().SingleOrDefaultAsync(x => x.StripeAccountId == stripeAccountId);
        account.Should().NotBeNull();

        var accountHistories = await DbContext.Set<AccountHistory>().Where(x => x.AccountId == account.Id).ToListAsync();
        accountHistories.Should().HaveCount(2);
    }
}