using FluentAssertions;
using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using NSubstitute;
using Stripe;
using Account = Lodgify.Payments.Stripe.Domain.Accounts.Account;
using IStripeClient = Lodgify.Payments.Stripe.Application.Services.IStripeClient;

namespace Lodgify.Payments.Stripe.Application.UnitTests.UseCases.CreateAccount;

public class CreateAccountCommandHandlerTests
{
    private IStripeClient _stripeClient;
    private IAccountRepository _accountRepository;
    private IUnitOfWork _unitOfWork;
    private CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _stripeClient = Substitute.For<IStripeClient>();
        _accountRepository = Substitute.For<IAccountRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateAccountCommandHandler(_stripeClient, _accountRepository, _unitOfWork);
    }

    private (CreateAccountCommandHandler handler, CreateAccountCommand request, CancellationToken cancellationToken) CreateHandlerAndDependencies()
    {
        var request = new CreateAccountCommand("US", "test@example.com") { Account = new LodgifyAccount(1, 1) };
        var cancellationToken = new CancellationToken();

        return (_handler, request, cancellationToken);
    }

    [Fact]
    public async Task Handle_ShouldCreateAccountAndCommit_WhenRequestIsValid()
    {
        // Arrange
        var (handler, request, cancellationToken) = CreateHandlerAndDependencies();
        var account = Account.Create(request.Account.UserId, "test@example.com", "acct_123", "application", "application", "stripe", "collection", "none");
        _stripeClient.CreateAccount(request.Account.UserId, "US", "test@example.com", Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        await _stripeClient.Received(1).CreateAccount(request.Account.UserId, request.Country, request.Email, cancellationToken);
        await _accountRepository.Received(1).AddAccountAsync(account, cancellationToken);
        await _unitOfWork.Received(1).CommitAsync(cancellationToken);
        Assert.Equal(account.StripeAccountId, response.StripeAccountId);
    }

    [Fact]
    public async Task Handle_ShouldThrowStripeException_WhenStripeClientThrowStripeException()
    {
        // Arrange
        var (handler, request, cancellationToken) = CreateHandlerAndDependencies();
        _stripeClient.When(s => s.CreateAccount(1, "US", "test@example.com", default))
            .Throw(new StripeException("Error"));

        // Act
        await handler.Invoking(s => s.Handle(request, cancellationToken))
            .Should()
            .ThrowAsync<StripeException>();

        // Assert
        await _accountRepository.DidNotReceive().AddAccountAsync(default, default);
        await _unitOfWork.DidNotReceive().CommitAsync(default);
    }
}