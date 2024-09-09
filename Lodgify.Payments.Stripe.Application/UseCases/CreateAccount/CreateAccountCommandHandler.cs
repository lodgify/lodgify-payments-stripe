using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Metrics;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, CreateAccountCommandResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateAccountCommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        AppMetrics.Account.Creating();

        try
        {
            var account = await _stripeClient.CreateAccountAsync(request.Account.UserId, request.Country, request.Email, cancellationToken);

            await _accountRepository.AddAccountAsync(account, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            AppMetrics.Account.Created();

            return new CreateAccountCommandResponse(account.StripeAccountId);
        }
        catch (Exception e)
        {
            AppMetrics.Account.Failed();
            throw;
        }        
    }
}