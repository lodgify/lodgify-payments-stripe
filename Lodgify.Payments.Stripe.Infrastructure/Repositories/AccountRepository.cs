﻿using System.Diagnostics.CodeAnalysis;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Domain.Accounts.Contracts;
using Lodgify.Payments.Stripe.Domain.Accounts.EntityViews;
using Microsoft.EntityFrameworkCore;

namespace Lodgify.Payments.Stripe.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentDbContext _dbContext;

    public AccountRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Account?> GetByStripeIdAsync(string stripeAccountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Account.FirstOrDefaultAsync(account => account.StripeAccountId == stripeAccountId, cancellationToken);
    }

    public async Task AddAccountAsync(Account account, CancellationToken cancellationToken)
    {
        await _dbContext.Account.AddAsync(account, cancellationToken);
    }

    public async Task<int?> QueryAccountUserIdAsync(string stripeAccountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Account
            .AsNoTracking()
            .Where(account => account.StripeAccountId == stripeAccountId)
            .Select(account => account.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<AccountView>> QueryUserAccountsAsync(int userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Account
            .AsNoTracking()
            .Where(account => account.UserId == userId)
            .Select(account => new AccountView(account.StripeAccountId, account.ChargesEnabled, account.DetailsSubmitted))
            .ToListAsync(cancellationToken);
    }
}