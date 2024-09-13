using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.WebhookEvents;
using Lodgify.Payments.Stripe.Domain.WebhookEvents.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;

public class AccountUpdatedCommandHandler : ICommandHandler<AccountUpdatedCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;
    private readonly IWebhookEventRepository _webhookEventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountUpdatedCommandHandler(IAccountRepository accountRepository, IAccountHistoryRepository accountHistoryRepository, IWebhookEventRepository webhookEventRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
        _webhookEventRepository = webhookEventRepository ?? throw new ArgumentNullException(nameof(webhookEventRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(AccountUpdatedCommand notification, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByStripeIdAsync(notification.AccountId, cancellationToken);
        if (account == null)
        {
            throw new AccountNotFoundException($"Account with id {notification.AccountId} not found");
        }

        account.Update(notification.ChargesEnabled, notification.DetailsSubmitted);

        try
        {
            await _accountHistoryRepository.AddAsync(AccountHistory.Create(account.Id, nameof(account.ChargesEnabled), account.ChargesEnabled.ToString(), notification.SourceEventId), cancellationToken);
            await _accountHistoryRepository.AddAsync(AccountHistory.Create(account.Id, nameof(account.DetailsSubmitted), account.DetailsSubmitted.ToString(), notification.SourceEventId), cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        var webhookEvent = WebhookEvent.Create(notification.SourceEventId, notification.RawSourceEventData);
        await _webhookEventRepository.AddAsync(webhookEvent, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}