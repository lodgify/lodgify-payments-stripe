using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public class CreateAccountSessionCommandHandler : ICommandHandler<CreateAccountSessionCommand, CreateAccountSessionResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CreateAccountSessionCommandHandler(IStripeClient stripeClient, IAccountSessionRepository sessionAccountRepository, IUnitOfWork unitOfWork)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _sessionAccountRepository = sessionAccountRepository ?? throw new ArgumentNullException(nameof(sessionAccountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateAccountSessionResponse> Handle(CreateAccountSessionCommand request, CancellationToken cancellationToken)
    {
        var account = await _stripeClient.CreateAccountSession(request.StripeAccountId, cancellationToken);

        await _sessionAccountRepository.AddAccountAsync(account, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreateAccountSessionResponse();
    }
}