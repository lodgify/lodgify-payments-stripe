using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;

public class AccountUpdatedCommandHandler : ICommandHandler<AccountUpdatedCommand>
{
    private readonly IAccountRepository _accountRepository;

    public AccountUpdatedCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task Handle(AccountUpdatedCommand notification, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByStripeIdAsync(notification.AccountId, cancellationToken);
        if (account == null)
        {
            throw new AccountNotFoundException($"Account with id {notification.AccountId} not found");
        }
    }
}