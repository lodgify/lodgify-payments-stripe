using Lodgify.Architecture.Metrics.Abstractions;
using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession.Rules;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public class CreateAccountSessionCommandHandler : ICommandHandler<CreateAccountSessionCommand, CreateAccountSessionResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMetricsClient _metrics;


    public CreateAccountSessionCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IAccountSessionRepository sessionAccountRepository, IUnitOfWork unitOfWork, IMetricsClient metrics)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _sessionAccountRepository = sessionAccountRepository ?? throw new ArgumentNullException(nameof(sessionAccountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
    }

    public async Task<CreateAccountSessionResponse> Handle(CreateAccountSessionCommand request, CancellationToken cancellationToken)
    {
        BusinessRule.CheckRule(new UserIdMustBeTheSameRule(request.Account.UserId, await _accountRepository.QueryAccountUserIdAsync(request.StripeAccountId, cancellationToken)));

        var account = await _stripeClient.CreateAccountSessionAsync(request.StripeAccountId, cancellationToken);

        await _sessionAccountRepository.AddAccountAsync(account, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        _metrics.Count(Metrics.Metrics.AccountSessionCreated, null);

        return new CreateAccountSessionResponse(account.StripeAccountId, account.ClientSecret);
    }
}