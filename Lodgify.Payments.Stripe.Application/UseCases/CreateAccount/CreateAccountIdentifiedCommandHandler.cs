using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Lodgify.Payments.Stripe.Domain.AccountHistories.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Metrics;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public class CreateAccountIdentifiedCommandHandler : IIdentifiedCommandHandler<CreateAccountIdentifiedCommand, CreateAccountCommandResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountIdentifiedCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IAccountHistoryRepository accountHistoryRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<CreateAccountCommandResponse> Handle(CreateAccountIdentifiedCommand request, CancellationToken cancellationToken)
    {
        AppMetrics.Account.Creating();

        try
        {
            var account = await _stripeClient.CreateAccountAsync(request.Account.UserId, request.Country, request.Email, cancellationToken);

            await _accountRepository.AddAccountAsync(account, cancellationToken);
            
             await _accountHistoryRepository.AddAsync(AccountHistory.CreateInitialHistory(account.Id, nameof(account.ChargesEnabled), account.ChargesEnabled.ToString(), account.CreatedAt), cancellationToken);
             await _accountHistoryRepository.AddAsync(AccountHistory.CreateInitialHistory(account.Id, nameof(account.DetailsSubmitted), account.DetailsSubmitted.ToString(), account.CreatedAt), cancellationToken);
            
            await _unitOfWork.CommitAsync(cancellationToken);

            AppMetrics.Account.Created();

            return new CreateAccountCommandResponse(account.StripeAccountId);
        }
        catch (Exception)
        {
            AppMetrics.Account.Failed();
            throw;
        }
    }
}