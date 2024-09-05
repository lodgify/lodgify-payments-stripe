using FluentAssertions;
using Lodgify.Architecture.Metrics.Abstractions;
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
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateAccountCommandHandler _handler;

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
        return (_handler, request, CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldCreateAccountAndCommit_WhenRequestIsValid()
    {
        // Arrange
        var (handler, request, cancellationToken) = CreateHandlerAndDependencies();
        var account = Account.Create(request.Account.UserId, "test@example.com", "acct_123", "application", "application", "stripe", "collection", "none");
        _stripeClient.CreateAccountAsync(request.Account.UserId, "US", "test@example.com", Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        await _stripeClient.Received(1).CreateAccountAsync(request.Account.UserId, request.Country, request.Email, cancellationToken);
        await _accountRepository.Received(1).AddAccountAsync(account, cancellationToken);
        await _unitOfWork.Received(1).CommitAsync(cancellationToken);
        Assert.Equal(account.StripeAccountId, response.StripeAccountId);
    }

    [Fact]
    public async Task Handle_ShouldThrowStripeException_WhenStripeClientThrowStripeException()
    {
        // Arrange
        var (handler, request, cancellationToken) = CreateHandlerAndDependencies();
        _stripeClient.When(s => s.CreateAccountAsync(1, "US", "test@example.com", CancellationToken.None))
            .Throw(new StripeException("Error"));

        // Act
        await handler.Invoking(s => s.Handle(request, cancellationToken))
            .Should()
            .ThrowAsync<StripeException>();

        // Assert
        await _accountRepository.DidNotReceiveWithAnyArgs().AddAccountAsync(Arg.Any<Account>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}