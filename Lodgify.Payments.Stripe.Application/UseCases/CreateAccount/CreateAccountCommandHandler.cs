using Lodgify.Architecture.Metrics.Abstractions;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMetricsClient _metrics;

    public CreateAccountCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMetricsClient metrics)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
    }

    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _stripeClient.CreateAccountAsync(request.Account.UserId, request.Country, request.Email, cancellationToken);

        await _accountRepository.AddAccountAsync(account, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _metrics.Count(Metrics.Metrics.AccountSessionCreated, null);

        return new CreateAccountResponse(account.StripeAccountId);
    }
}