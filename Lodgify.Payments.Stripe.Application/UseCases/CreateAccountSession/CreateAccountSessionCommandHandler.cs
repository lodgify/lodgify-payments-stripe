using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public class CreateAccountSessionCommandHandler : ICommandHandler<CreateAccountSessionCommand, CreateAccountSessionResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CreateAccountSessionCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IAccountSessionRepository sessionAccountRepository, IUnitOfWork unitOfWork)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _sessionAccountRepository = sessionAccountRepository ?? throw new ArgumentNullException(nameof(sessionAccountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateAccountSessionResponse> Handle(CreateAccountSessionCommand request, CancellationToken cancellationToken)
    {
        var accountUserId = await _accountRepository.QueryAccountUserIdAsync(request.StripeAccountId, cancellationToken);
        if (!accountUserId.HasValue || accountUserId != request.Account.UserId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create a session for this account");
        }

        var account = await _stripeClient.CreateAccountSession(request.StripeAccountId, cancellationToken);

        await _sessionAccountRepository.AddAccountAsync(account, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreateAccountSessionResponse(account.StripeAccountId, account.ClientSecret);
    }
}