using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Infrastructure;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Lodgify.Payments.Stripe.Server.IntegrationTests.WireMock.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1.Externall;

public class AccountControllerTests : BaseIntegrationTest
{
    private const string RequestBaseUrl = "api/v1/accounts";

    public AccountControllerTests(CustomWebApplicationFactory factory) : base(factory)
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

    [Fact]
    public async Task Should_Throw_DbUpdateConcurrencyException_On_Conflict()
    {
        //Arrange
        const int accountId = 5;
        var account = PredefinedMocks.AccountMock.Create(accountId);
        await Insert(account);

        var _dbContextOptions = new DbContextOptionsBuilder<PaymentDbContext>()
            .UseNpgsql(DatabaseConnectionString)
            .Options;


        using (var context1 = new PaymentDbContext(_dbContextOptions))
        using (var context2 = new PaymentDbContext(_dbContextOptions))
        {
            var account1 = await context1.Account.FirstAsync(p => p.UserId == accountId);
            var account2 = await context2.Account.FirstAsync(p => p.UserId == accountId);

            account1.SetChargesEnabled(true, DateTime.UtcNow);
            await context1.SaveChangesAsync();

            account2.SetDetailsSubmitted(true, DateTime.UtcNow);

            await context2.Invoking(s => s.SaveChangesAsync())
                .Should()
                .ThrowAsync<DbUpdateConcurrencyException>();
        }
    }

    private async Task AssertAccountHistoryCreated(string stripeAccountId)
    {
        var account = await DbContext.Set<Account>().Where(x => x.StripeAccountId == stripeAccountId).FirstOrDefaultAsync();
        account.Should().NotBeNull();

        var accountHistories = await DbContext.Set<AccountHistory>().Where(x => x.AccountId == account.Id).ToListAsync();
        accountHistories.Should().HaveCount(2);
    }
}