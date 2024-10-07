using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Stripe;
using AccountSession = Lodgify.Payments.Stripe.Domain.AccountSessions.AccountSession;

namespace Lodgify.Payments.Stripe.Application.UnitTests.UseCases.CreateAccountSession;

public class CreateAccountSessionIdentifiedCommandHandlerTests
{
    private readonly IStripeGatewayClient _stripeGatewayClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateAccountSessionIdentifiedCommandHandler _handler;

    public CreateAccountSessionIdentifiedCommandHandlerTests()
    {
        _stripeGatewayClient = Substitute.For<IStripeGatewayClient>();
        _accountRepository = Substitute.For<IAccountRepository>();
        _sessionAccountRepository = Substitute.For<IAccountSessionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateAccountSessionIdentifiedCommandHandler(_stripeGatewayClient, _accountRepository, _sessionAccountRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsCreateAccountSessionResponse()
    {
        // Arrange
        var request = new CreateAccountSessionIdentifiedCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };
        var account = AccountSession.Create("stripeAccountId", "clientSecret");

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId);

        _stripeGatewayClient.CreateAccountSessionAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("stripeAccountId", response.StripeAccountId);
        Assert.Equal("clientSecret", response.ClientSecret);
    }

    [Fact]
    public async Task Handle_BusinessRuleViolation_ThrowsBusinessRuleException()
    {
        // Arrange
        var request = new CreateAccountSessionIdentifiedCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId + 1);

        // Act and Assert
        await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }


    [Fact]
    public async Task Handle_StripeClientThrowsException_ThrowsException()
    {
        // Arrange
        var request = new CreateAccountSessionIdentifiedCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId);

        _stripeGatewayClient.CreateAccountSessionAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new StripeException("Error creating account session"));

        // Act and Assert
        await Assert.ThrowsAsync<StripeException>(() => _handler.Handle(request, CancellationToken.None));
    }
}