﻿using Lodgify.Payments.Stripe.Application.BuildingBlocks;
using Lodgify.Payments.Stripe.Application.Services;
using Lodgify.Payments.Stripe.Application.Transactions;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession.Rules;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.AccountSessions.Contracts;
using Lodgify.Payments.Stripe.Metrics;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;

public class CreateAccountSessionIdentifiedCommandHandler : IIdentifiedCommandHandler<CreateAccountSessionIdentifiedCommand, CreateAccountSessionCommandResponse>
{
    private readonly IStripeClient _stripeClient;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSessionRepository _sessionAccountRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CreateAccountSessionIdentifiedCommandHandler(IStripeClient stripeClient, IAccountRepository accountRepository, IAccountSessionRepository sessionAccountRepository, IUnitOfWork unitOfWork)
    {
        _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _sessionAccountRepository = sessionAccountRepository ?? throw new ArgumentNullException(nameof(sessionAccountRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateAccountSessionCommandResponse> Handle(CreateAccountSessionIdentifiedCommand request, CancellationToken cancellationToken)
    {
        AppMetrics.AccountSession.Creating();

        try
        {
            BusinessRule.CheckRule(new UserIdMustBeTheSameRule(request.Account.UserId, await _accountRepository.QueryAccountUserIdAsync(request.StripeAccountId, cancellationToken)));

            var account = await _stripeClient.CreateAccountSessionAsync(request.StripeAccountId, cancellationToken);

            await _sessionAccountRepository.AddAccountAsync(account, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            AppMetrics.AccountSession.Created();

            return new CreateAccountSessionCommandResponse(account.StripeAccountId, account.ClientSecret);
        }
        catch (Exception)
        {
            AppMetrics.AccountSession.Failed();
            throw;
        }        
    }
}