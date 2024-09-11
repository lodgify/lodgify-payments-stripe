using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts.EntityViews;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Lodgify.Payments.Stripe.Application.UnitTests.UseCases.GetAccounts;

public class GetAccountsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsGetAccountsQueryResponse()
    {
        // Arrange
        var request = new GetAccountsQuery() { Account = new LodgifyAccount(1, 1) };
        var responseView = new List<AccountView> { new AccountView("acct_123", false, false) };
        var accountRepository = Substitute.For<IAccountRepository>();
        var queryHandler = new GetAccountsQueryHandler(accountRepository);

        accountRepository.QueryUserAccountsAsync(1, Arg.Any<CancellationToken>())
            .Returns(responseView);

        // Act
        var response = await queryHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Accounts);
        Assert.Equal(responseView[0].StripeAccountId, response.Accounts[0].StripeAccountId);
        Assert.Equal(responseView[0].ChargesEnabled, response.Accounts[0].ChargesEnabled);
        Assert.Equal(responseView[0].DetailsSubmitted, response.Accounts[0].DetailsSubmitted);
    }

    [Fact]
    public async Task Handle_AccountRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var request = new GetAccountsQuery() { Account = new LodgifyAccount(1, 1) };
        var accountRepository = Substitute.For<IAccountRepository>();
        var queryHandler = new GetAccountsQueryHandler(accountRepository);

        accountRepository.QueryUserAccountsAsync(1, Arg.Any<CancellationToken>())
            .Throws<Exception>();

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => queryHandler.Handle(request, CancellationToken.None));
    }
}