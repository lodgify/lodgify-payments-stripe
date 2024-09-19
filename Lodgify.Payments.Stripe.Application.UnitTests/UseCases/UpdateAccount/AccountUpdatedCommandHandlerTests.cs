using FluentAssertions;
using Lodgify.Extensions.Primitives.Identity;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.WebhookEvents;
using Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;
using NSubstitute;

namespace Lodgify.Payments.Stripe.Application.UnitTests.UseCases.UpdateAccount;

public class AccountUpdatedCommandHandlerTests
{
    private IAccountRepository accountRepository;
    private IAccountHistoryRepository accountHistoryRepository;
    private IWebhookEventRepository webhookEventRepository;
    private IUnitOfWork unitOfWork;
    private AccountUpdatedCommandHandler sut = null!;

    public AccountUpdatedCommandHandlerTests()
    {
        accountRepository = Substitute.For<IAccountRepository>();
        accountHistoryRepository = Substitute.For<IAccountHistoryRepository>();
        webhookEventRepository = Substitute.For<IWebhookEventRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
    }

    [Fact]
    public async Task Account_Updated_ChargesEnabledChanged_DetailsSubmittedChanged_Success()
    {
        // Arrange
        var stripeAccountId = "acc_123";
        var stripeRawEvent = "{\"id\":\"evt_123\",\"object\":\"event\"}";
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", stripeAccountId, "stripe", "stripe", "stripe", "stripe", "none", false, false, accountCreatedAt);

        webhookEventRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        accountRepository.GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>()).Returns(account);

        var updateAccountEventCreatedAt = DateTime.UtcNow;
        var command = new AccountUpdatedCommand(stripeAccountId, true, true, stripeRawEvent, "sourceEventId", updateAccountEventCreatedAt);

        sut = new AccountUpdatedCommandHandler(accountRepository, accountHistoryRepository, webhookEventRepository, unitOfWork);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().Be(updateAccountEventCreatedAt);
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().Be(updateAccountEventCreatedAt);
        
        await accountRepository.Received(1).GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>());
        await accountHistoryRepository.Received(2).AddAsync(Arg.Any<AccountHistory>(), Arg.Any<CancellationToken>());
        await webhookEventRepository.Received(1).AddAsync(Arg.Any<WebhookEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Account_Updated_ChargesEnabledChanged_Success()
    {
        // Arrange
        var stripeAccountId = "acc_123";
        var stripeRawEvent = "{\"id\":\"evt_123\",\"object\":\"event\"}";
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", stripeAccountId, "stripe", "stripe", "stripe", "stripe", "none", false, false, accountCreatedAt);

        webhookEventRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        accountRepository.GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>()).Returns(account);

        var updateAccountEventCreatedAt = DateTime.UtcNow;
        var command = new AccountUpdatedCommand(stripeAccountId, true, false, stripeRawEvent, "sourceEventId", updateAccountEventCreatedAt);

        sut = new AccountUpdatedCommandHandler(accountRepository, accountHistoryRepository, webhookEventRepository, unitOfWork);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().Be(updateAccountEventCreatedAt);
        account.DetailsSubmitted.Should().BeFalse();
        account.DetailsSubmittedSetAt.Should().NotBe(updateAccountEventCreatedAt);
        
        await accountRepository.Received(1).GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>());
        await accountHistoryRepository.Received(1).AddAsync(Arg.Any<AccountHistory>(), Arg.Any<CancellationToken>());
        await webhookEventRepository.Received(1).AddAsync(Arg.Any<WebhookEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Account_Updated_DetailsSubmittedChanged_Success()
    {
        // Arrange
        var stripeAccountId = "acc_123";
        var stripeRawEvent = "{\"id\":\"evt_123\",\"object\":\"event\"}";
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", stripeAccountId, "stripe", "stripe", "stripe", "stripe", "none", false, false, accountCreatedAt);

        webhookEventRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        accountRepository.GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>()).Returns(account);

        var updateAccountEventCreatedAt = DateTime.UtcNow;
        var command = new AccountUpdatedCommand(stripeAccountId, false, true, stripeRawEvent, "sourceEventId", updateAccountEventCreatedAt);

        sut = new AccountUpdatedCommandHandler(accountRepository, accountHistoryRepository, webhookEventRepository, unitOfWork);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        account.ChargesEnabled.Should().BeFalse();
        account.ChargesEnabledSetAt.Should().NotBe(updateAccountEventCreatedAt);
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().Be(updateAccountEventCreatedAt);
        
        await accountRepository.Received(1).GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>());
        await accountHistoryRepository.Received(1).AddAsync(Arg.Any<AccountHistory>(), Arg.Any<CancellationToken>());
        await webhookEventRepository.Received(1).AddAsync(Arg.Any<WebhookEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Account_Updated_ChargesEnabledNotChanged_DetailsSubmittedNotChanged_Success()
    {
        // Arrange
        var stripeAccountId = "acc_123";
        var stripeRawEvent = "{\"id\":\"evt_123\",\"object\":\"event\"}";
        var accountCreatedAt = DateTime.UtcNow;
        var account = Account.Create(1, "testuser@example.com", stripeAccountId, "stripe", "stripe", "stripe", "stripe", "none", true, true, accountCreatedAt);

        webhookEventRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        accountRepository.GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>()).Returns(account);

        var updateAccountEventCreatedAt = DateTime.UtcNow;
        var command = new AccountUpdatedCommand(stripeAccountId, true, true, stripeRawEvent, "sourceEventId", updateAccountEventCreatedAt);

        sut = new AccountUpdatedCommandHandler(accountRepository, accountHistoryRepository, webhookEventRepository, unitOfWork);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        account.ChargesEnabled.Should().BeTrue();
        account.ChargesEnabledSetAt.Should().NotBe(updateAccountEventCreatedAt);
        account.DetailsSubmitted.Should().BeTrue();
        account.DetailsSubmittedSetAt.Should().NotBe(updateAccountEventCreatedAt);
        
        await accountRepository.Received(1).GetByStripeIdAsync(stripeAccountId, Arg.Any<CancellationToken>());
        await accountHistoryRepository.DidNotReceive().AddAsync(Arg.Any<AccountHistory>(), Arg.Any<CancellationToken>());
        await webhookEventRepository.Received(1).AddAsync(Arg.Any<WebhookEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Account_NotUpdated_Event_Already_Processed_Success()
    {
         // Arrange
        var stripeAccountId = "acc_123";
        var stripeRawEvent = "{\"id\":\"evt_123\",\"object\":\"event\"}";
        
        webhookEventRepository.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        var command = new AccountUpdatedCommand(stripeAccountId, true, true, stripeRawEvent, "sourceEventId", DateTime.Now);
        
        sut = new AccountUpdatedCommandHandler(accountRepository, accountHistoryRepository, webhookEventRepository, unitOfWork);

        //Act
        await sut.Handle(command, CancellationToken.None);
        
        // Assert
        await webhookEventRepository.Received(1).ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await webhookEventRepository.DidNotReceive().AddAsync(Arg.Any<WebhookEvent>(), Arg.Any<CancellationToken>());
        await unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        await accountHistoryRepository.DidNotReceive().AddAsync(Arg.Any<AccountHistory>(), Arg.Any<CancellationToken>());
    }
}