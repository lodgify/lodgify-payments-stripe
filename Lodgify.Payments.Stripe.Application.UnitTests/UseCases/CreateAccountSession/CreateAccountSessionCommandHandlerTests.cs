using Lodgify.Architecture.Metrics.Abstractions;
using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Stripe;
using AccountSession = Lodgify.Payments.Stripe.Domain.AccountSessions.AccountSession;
using IStripeClient = Lodgify.Payments.Stripe.Application.Services.IStripeClient;

namespace Lodgify.Payments.Stripe.Application.UnitTests.UseCases.CreateAccountSession;

public class CreateAccountSessionCommandHandlerTests
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMetricsClient _metrics;
    private readonly CreateAccountSessionCommandHandler _handler;

    public CreateAccountSessionCommandHandlerTests()
    {
        _stripeClient = Substitute.For<IStripeClient>();
        _accountRepository = Substitute.For<IAccountRepository>();
        _sessionAccountRepository = Substitute.For<IAccountSessionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _metrics = Substitute.For<IMetricsClient>();
        _handler = new CreateAccountSessionCommandHandler(_stripeClient, _accountRepository, _sessionAccountRepository, _unitOfWork, _metrics);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsCreateAccountSessionResponse()
    {
        // Arrange
        var request = new CreateAccountSessionCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };
        var account = AccountSession.Create("stripeAccountId", "clientSecret");

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId);

        _stripeClient.CreateAccountSessionAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
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
        var request = new CreateAccountSessionCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId + 1);

        // Act and Assert
        await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }


    [Fact]
    public async Task Handle_StripeClientThrowsException_ThrowsException()
    {
        // Arrange
        var request = new CreateAccountSessionCommand("stripeAccountId") { Account = new LodgifyAccount(1, 1) };

        _accountRepository.QueryAccountUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(request.Account.UserId);

        _stripeClient.CreateAccountSessionAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new StripeException("Error creating account session"));

        // Act and Assert
        await Assert.ThrowsAsync<StripeException>(() => _handler.Handle(request, CancellationToken.None));
    }
}